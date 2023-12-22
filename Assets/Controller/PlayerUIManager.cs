using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : PlayerView
{

    [SerializeField] Button[] respawnButtons;
    [SerializeField] Button[] exitButtons;

    GameObject consoleCanvas;
    public GameObject ConsoleCanvas { get => consoleCanvas; set => consoleCanvas = value; }

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
            ConsoleCanvas = RoomManager.Instance.ConsoleCanvas;
        }
    }

    public void OnViewConsole()
    {
        if (ConsoleCanvas == null) return;
        isConsoleShowing = !isConsoleShowing;
        if (isConsoleShowing)
        {
            ConsoleCanvas.SetActive(true);
        }
        else
        {
            ConsoleCanvas.SetActive(false);
        }
    }

    #endregion
}
