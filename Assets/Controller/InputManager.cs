using UnityEngine;

public class InputManager : PlayerView
{
    InputActions input;
    InputActions.PlayerActionActions playerAction;

    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    PlayerUIManager playerUIManager;

    [SerializeField]
    GameObject[] objectsToDestroy, objectsToEnable;

    Scoreboard scoreboard;

    Vector2 movement, look;
    float zoom;

    bool isPlayable()
    {
        return view.IsMine;
    }
    private void Awake()
    {
        if (!isPlayable())
        {
            foreach (GameObject obj in objectsToDestroy)
            {
                Destroy(obj);
            }
            enabled = false;
            return;
        }

        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }

        input = new InputActions();

        scoreboard = RoomManager.Instance.Scoreboard;

        playerAction = input.PlayerAction;

        playerAction.Movement.performed += ctx => movement = ctx.ReadValue<Vector2>();

        playerAction.MouseX.performed += ctx => look.x = ctx.ReadValue<float>();
        playerAction.MouseY.performed += ctx => look.y = ctx.ReadValue<float>();

        playerAction.Zoom.performed += ctx => playerController.ReceiveZoom(ctx.ReadValue<float>());

        playerAction.Jump.performed += _ => playerController.OnJumpPressed();

        playerAction.Right.performed += _ => playerController.OnScope();
        playerAction.Fire.performed += _ => playerController.OnFire();

        playerAction.Back.performed += _ => playerController.OnViewMenu();

        playerAction.Scoreboard.performed += _ => scoreboard.OnViewScoreboard();

        playerAction.Console.performed += _ => playerUIManager.OnViewConsole();
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
