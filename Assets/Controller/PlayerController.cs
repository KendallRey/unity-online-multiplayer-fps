using UnityEngine;

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
    [SerializeField] Transform footPosition;
    float xRot = 0f;
    bool isGrounded, jump;
    CharacterController characterController;

    float mouseX, mouseY;
    Vector2 look, move;
    Vector3 down = Vector3.zero;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ReceiveMovement(Vector2 movement)
    {
        move = movement;
    }
    public void ReceiveLook(Vector2 look)
    {
        mouseX = look.x * lookSensitivityX;
        mouseY = look.y * lookSensitivityY;
    }
    public void OnJumpPressed()
    {
        jump = true;
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
