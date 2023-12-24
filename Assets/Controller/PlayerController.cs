using UnityEngine;
using Photon.Pun;

public class PlayerController : PlayerView
{
    [Header("Move Config")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpHeight = 4f;
    [SerializeField] float gravity = -30f;


    [Header("Look Config")]
    [SerializeField] Transform cameraContainer;

    [SerializeField] const float defaultLookSensitivityX = 12f;
    [SerializeField] const float defaultLookSensitivityY = 0.2f;
    [SerializeField] float xClamp = 85f;
    [SerializeField] float zoomSensitivity = 0.2f;
    [SerializeField] float maxZoom = 5f, minZoom = 40f;
    [SerializeField] Camera playerCamera;

    float lookSensitivityX = defaultLookSensitivityX;
    float lookSensitivityY = defaultLookSensitivityY;

    const float defaultZoom = 60f;
    float currentZoom = defaultZoom;

    [Header("Settings")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform footPosition, gunPosition;
    [SerializeField] float reloadTime = 2f;
    [SerializeField] float reloadAudioTime = 1.5f;
    [SerializeField] AudioSource reloadAudioSource;
    float xRot = 0f, currentReloadTime = 0f;
    bool isGrounded, jump, isReloaded = true;
    CharacterController characterController;
    HealthManager healthManager;
    [SerializeField] SFXManager sfxManager;
    [SerializeField] FXManager fxManager;

    [Header("HUD")]
    [SerializeField] GameObject scopePanel;
    [SerializeField] GameObject menuPanel;


    bool isMenuView = false;
    bool isScoped = false;


    float mouseX, mouseY;
    Vector2 look, move;
    Vector3 down = Vector3.zero;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        healthManager = GetComponent<HealthManager>();
    }
    void Start()
    {
        if (!view.IsMine)
        {
            enabled = false;
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ReceiveMovement(Vector2 movement)
    {
        if (isMenuView || healthManager.IsDead || !view.IsMine) return;
        move = movement;
    }
    public void ReceiveLook(Vector2 look)
    {
        if (isMenuView || healthManager.IsDead || !view.IsMine) return;
        mouseX = look.x * lookSensitivityX;
        mouseY = look.y * lookSensitivityY;
    }
    public void OnJumpPressed()
    {
        if (isMenuView || healthManager.IsDead || !view.IsMine) return;
        jump = true;
    }

    public void OnScope()
    {
        if (isMenuView || healthManager.IsDead || !view.IsMine) return;
        isScoped = !isScoped;
        if (isScoped)
        {
            scopePanel.SetActive(true);
            currentZoom = minZoom;
        }
        else
        {
            scopePanel.SetActive(false);
            currentZoom = defaultZoom;
            playerCamera.fieldOfView = defaultZoom;
            lookSensitivityX = defaultLookSensitivityX;
            lookSensitivityY = defaultLookSensitivityY;
        }
    }

    public void ReceiveZoom(float zoom)
    {
        if (isMenuView || healthManager.IsDead || !view.IsMine) return;
        if (!isScoped) return;
        float newZoom = currentZoom + (-zoom * zoomSensitivity);
        float clampedZoom = Mathf.Clamp(newZoom, maxZoom, minZoom);
        float percentage = Mathf.InverseLerp(maxZoom, minZoom, clampedZoom) + 0.1f;

        lookSensitivityY = defaultLookSensitivityY * percentage;
        lookSensitivityX = defaultLookSensitivityX * percentage;

        currentZoom = clampedZoom;
    }
    public void OnFire()
    {
        if (!isReloaded) return;
        if (isMenuView || healthManager.IsDead || !view.IsMine) return;
        Ray ray = new(gunPosition.position, gunPosition.forward);
        currentReloadTime = reloadTime;
        isReloaded = false;
        sfxManager.OnFireSFX(ray.origin);
        reloadAudioSource.PlayDelayed(reloadTime - reloadAudioTime);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.tag == "Border") return;
            sfxManager.OnBulletHitSFX(hit.collider, hit.point);
            fxManager.OnBulletHitFX(hit.collider, hit.point, ray.origin);
            hit.collider.TryGetComponent(out PhotonView playerView);
            if (playerView == null) return;
            OnHitPlayer(playerView);
        }
    }

    void OnHitPlayer(PhotonView playerView)
    {
        playerView.RPC(nameof(HealthManager.RPC_OnTakeDamage), playerView.Owner, 120f);
    }

    public void OnViewMenu()
    {
        if (healthManager.IsDead || !view.IsMine) return;
        isMenuView = !isMenuView;
        if (isMenuView)
        {
            Cursor.lockState = CursorLockMode.None;
            menuPanel.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            menuPanel.SetActive(false);
        }
    }
    void Update()
    {
        Reload();
        if (!view.IsMine || healthManager.IsDead) return;
        Move();
        Look();
        if (!isScoped) return;
        Zoom();
    }

    void Reload()
    {
        if (!isReloaded) currentReloadTime -= Time.deltaTime;
        if (currentReloadTime <= 0)
            isReloaded = true;
    }
    void Move()
    {
        isGrounded = Physics.CheckSphere(footPosition.position, 0.50f, groundLayer);
        if (isGrounded)
        {
            down.y = 0;
        }
        Vector3 _movement = (transform.right * move.x + transform.forward * move.y) * moveSpeed;
        characterController.Move(Time.deltaTime * _movement);

        if (jump)
        {
            if (isGrounded)
            {
                down.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }
            jump = false;
        }
        down.y += gravity * Time.deltaTime;
        characterController.Move(Time.deltaTime * down);
    }
    void Look()
    {
        transform.Rotate(Vector3.up, mouseX * Time.deltaTime);

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -xClamp, xClamp);

        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRot;
        cameraContainer.eulerAngles = targetRotation;
    }

    void Zoom()
    {
        playerCamera.fieldOfView = currentZoom;
    }
}
