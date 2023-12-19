using UnityEngine;
using Photon.Pun;

public class InputManager : MonoBehaviour
{
    InputActions input;
    InputActions.PlayerActionActions playerAction;

    [SerializeField]
    PlayerController playerController;

    PhotonView view;
    bool isDebug = false;

    Vector2 movement, look;

    bool isPlayable()
    {
        if (isDebug) return true;
        return view != null && view.IsMine;
    }

    private void Awake()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
            isDebug = true;
        
        view = GetComponent<PhotonView>();

        if (!isPlayable())
        {
            enabled = false;
            return;
        }

        playerController.View = view;

        input = new InputActions();
        playerAction = input.PlayerAction;

        playerAction.Movement.performed += ctx => movement = ctx.ReadValue<Vector2>();

        playerAction.MouseX.performed += ctx => look.x = ctx.ReadValue<float>();
        playerAction.MouseY.performed += ctx => look.y = ctx.ReadValue<float>();

        playerAction.Jump.performed += _ => playerController.OnJumpPressed();

        playerAction.Right.performed += _ => playerController.OnScope();
        playerAction.Fire.performed += _ => playerController.OnFire();

        playerAction.Back.performed += _ => playerController.OnViewMenu();
    }

    private void OnEnable()
    {
        if (isPlayable())
            input.Enable();
    }

    private void OnDisable()
    {
        if(isPlayable())
            input.Disable();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (!isPlayable())
        {
            return;
        }
        playerController.ReceiveMovement(movement);
        playerController.ReceiveLook(look);
    }
}
