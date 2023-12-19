using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [Header("Move Config")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpHeight = 4f;
    [SerializeField] float gravity = -30f;

    [Header("Look Config")]
    [SerializeField] Transform cameraContainer;
    [SerializeField] float lookSensitivityX = 8f;
    [SerializeField] float lookSensitivityY = 0.5f;
    [SerializeField] float xClamp = 85f;

    [Header("Settings")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform footPosition, gunPosition;
    float xRot = 0f;
    bool isGrounded, jump;
    CharacterController characterController;
    HealthManager healthManager;

    [Header("HUD")]
    [SerializeField] GameObject scopePanel;
    [SerializeField] GameObject menuPanel;


    bool isMenuView = false;
    bool isScoped = false;
    PhotonView view;
    public PhotonView View { get => view; set => view = value; }

    SpawnPlayers spawner;
    public SpawnPlayers Spawner { get => spawner; set => spawner = value; }

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
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetSpawner(SpawnPlayers spawner)
    {
        Spawner = spawner;
        healthManager.Spawner = spawner;
    }

    public void ReceiveMovement(Vector2 movement)
    {
        if (isMenuView || healthManager.IsDead) return;
        move = movement;
    }
    public void ReceiveLook(Vector2 look)
    {
        if (isMenuView || healthManager.IsDead) return;
        mouseX = look.x * lookSensitivityX;
        mouseY = look.y * lookSensitivityY;
    }
    public void OnJumpPressed()
    {
        if (isMenuView || healthManager.IsDead) return;
        jump = true;
    }

    public void OnScope()
    {
        if (isMenuView || healthManager.IsDead) return;
        isScoped = !isScoped;
        if (isScoped)
        {
            scopePanel.SetActive(true);
        }
        else
        {
            scopePanel.SetActive(false);
        }
    }
    public void OnFire()
    {
        if (isMenuView || healthManager.IsDead) return;
        Ray ray = new(gunPosition.position, gunPosition.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit Name: " + hit.collider.name);
            Debug.Log("Hit Tag: " + hit.collider.tag);
            hit.collider.TryGetComponent(out PhotonView playerView);
            if (playerView == null) return;
            OnHitPlayer(playerView);
        }
    }

    void OnHitPlayer(PhotonView playerView)
    {
        View.RPC("OnTakeDamage", playerView.Owner, 120f, View.ViewID);
    }
    public void OnViewMenu()
    {
        if (healthManager.IsDead) return;
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

 
    // Update is called once per frame
    void Update()
    {
        Move();
        Look();
    }
    void Move()
    {
        isGrounded = Physics.CheckSphere(footPosition.position, 0.2f, groundLayer);
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
}
