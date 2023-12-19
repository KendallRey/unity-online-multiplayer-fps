using UnityEngine;

public class InputManager : MonoBehaviour
{
    InputActions input;
    InputActions.PlayerActionActions playerAction;

    [SerializeField]
    PlayerController playerController;

    Vector2 movement, look;
    private void Awake()
    {
        input = new InputActions();
        playerAction = input.PlayerAction;

        playerAction.Movement.performed += ctx => movement = ctx.ReadValue<Vector2>();

        playerAction.MouseX.performed += ctx => look.x = ctx.ReadValue<float>();
        playerAction.MouseY.performed += ctx => look.y = ctx.ReadValue<float>();

        playerAction.Jump.performed += _ => playerController.OnJumpPressed();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerController.ReceiveMovement(movement);
        playerController.ReceiveLook(look);
    }
}
