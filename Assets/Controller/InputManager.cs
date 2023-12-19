using UnityEngine;
using Photon.Pun;

public class InputManager : MonoBehaviour
{
    InputActions input;
    InputActions.PlayerActionActions playerAction;

    [SerializeField]
    PlayerController playerController;

    [SerializeField]
    GameObject[] objectsToDestroy;

    PhotonView view;

    Vector2 movement, look;

    bool isPlayable()
    {
        return view != null && view.IsMine;
    }

    private void Awake()
    {
        
        view = GetComponent<PhotonView>();

        if (!isPlayable())
        {
            enabled = false;
            playerController.enabled = false;
            foreach(GameObject obj in objectsToDestroy)
            {
                Destroy(obj);
            }
            return;
        }

        playerController.SetView(view);

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
        if (!isPlayable()) return;
        if (input != null)
            input.Enable();
    }

    private void OnDisable()
    {
        if (!isPlayable()) return;
        if(input != null)
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
