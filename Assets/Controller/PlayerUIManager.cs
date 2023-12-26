using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerUIManager : PlayerView
{

    [SerializeField] Button[] respawnButtons;
    [SerializeField] Button[] quitButtons;
    [SerializeField] Button[] exitButtons;

    ConsoleManager consoleManager;
    public ConsoleManager ConsoleMananger { get => consoleManager; set => consoleManager = value; }

    bool isConsoleShowing = false;
    private void Start()
    {
        SetConsole();
        SetRespawnButtons();
        SetQuitButtons();
        SetExitButtons();
    }

    void SetRespawnButtons()
    {
        foreach(Button button in respawnButtons)
        {
            button.onClick.AddListener(() => playerManager.RespawnPlayerOnline(gameObject));
        }
    }
    void SetExitButtons()
    {
        foreach (Button button in exitButtons)
        {
            button.onClick.AddListener(() => RoomManager.Instance.ExitGame());
        }
    }
    void SetQuitButtons()
    {
        foreach (Button button in quitButtons)
        {
            button.onClick.AddListener(() => playerManager.QuitGame());
        }
    }
    #region Console

    void SetConsole()
    {
        bool isConsoleEnabled = PlayerPrefs.GetString("ConsoleEnabled", "NO") == "ConsoleEnabled";
        if (isConsoleEnabled)
        {
            Debug.Log("Console Commands Enabled");
            consoleManager = RoomManager.Instance.ConsoleManager;
            consoleManager.ConsoleInput.onEndEdit.AddListener(OnEnterCommand);
        }
    }

    public void OnViewConsole()
    {
        if (consoleManager == null) return;
        isConsoleShowing = !isConsoleShowing;
        if (isConsoleShowing)
        {
            consoleManager.SetConsoleCanvasActive(true);
        }
        else
        {
            consoleManager.SetConsoleCanvasActive(false);
        }
    }


    #endregion

    #region Commands

    const string MakeMeInvisible = "MakeMeInvisible";
    public void OnEnterCommand(string commandText)
    {
        if (!view.IsMine) return;
        string[] commands = commandText.Split(" ");
        if (commands.Length != 2) return;
        bool isTrue = commandText.Contains("true");
        switch (commands[0])
        {
            case MakeMeInvisible:
                consoleManager.AddConsoleItem(commandText);
                view.RPC(nameof(RPC_MakeMeInvisible), RpcTarget.Others, isTrue);
                break;
        }
    }

    [PunRPC]
    public void RPC_MakeMeInvisible(bool isTrue)
    {
        if (view.IsMine) return;
        MeshRenderer[] meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = !isTrue;
        }
    }

    #endregion
}
