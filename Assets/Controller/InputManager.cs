using UnityEngine;
using Photon.Pun;

public class InputManager : MonoBehaviour
{
    InputActions input;
    InputActions.PlayerActionActions playerAction;

    [SerializeField]
    PlayerController playerController;

    PhotonView view;

    Vector2 movement, look;
    private void Awake()
    {
        view = GetComponent<PhotonView>();
        if(!view.IsMine)
        {
            enabled = false;
            return;
        }

        input = new InputActions();
        playerAction = input.PlayerAction;

        playerAction.Movement.performed += ctx => movement = ctx.ReadValue<Vector2>();

        playerAction.MouseX.performed += ctx => look.x = ctx.ReadValue<float>();
        playerAction.MouseY.performed += ctx => look.y = ctx.ReadValue<float>();

        playerAction.Jump.performed += _ => playerController.OnJumpPressed();
    }

    private void OnEnable()
    {
        if (!view.IsMine)
        {
            return;
        }
        input.Enable();
    }

    private void OnDisable()
    {
        if (!view.IsMine)
        {
            return;
        }
        input.Disable();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!view.IsMine)
        {
            return;
        }
        playerController.ReceiveMovement(movement);
        playerController.ReceiveLook(look);
    }
}
