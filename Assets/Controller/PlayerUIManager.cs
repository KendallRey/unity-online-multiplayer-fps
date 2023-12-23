using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : PlayerView
{

    [SerializeField] Button[] respawnButtons;
    [SerializeField] Button[] exitButtons;

    ConsoleManager consoleManager;
    public ConsoleManager ConsoleMananger { get => consoleManager; set => consoleManager = value; }

    bool isConsoleShowing = false;
    private void Start()
    {
        SetConsole();
        SetRespawnButtons();
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
    #region Console

    void SetConsole()
    {
        bool isConsoleEnabled = PlayerPrefs.GetString("ConsoleEnabled", "NO") == "ConsoleEnabled";
        if (isConsoleEnabled)
        {
            Debug.Log("Console Commands Enabled");
            consoleManager = RoomManager.Instance.ConsoleManager;
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
            consoleManager.SetConsoleCanvasActive(true);
        }
    }

    #endregion
}
