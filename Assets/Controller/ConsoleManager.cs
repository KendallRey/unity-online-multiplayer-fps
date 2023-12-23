using UnityEngine;
using TMPro;

public class ConsoleManager : MonoBehaviour
{
    #region TEST CONSOLE COMMANDS
    /* YEAH, YOU CAN PUT CHEATS HERE*/

    #endregion
    
    [SerializeField] TMP_InputField consoleInput;
    [SerializeField] GameObject consoleCanvas;

    public void OnEnterCommand()
    {
        Debug.Log("Command: " + consoleInput.text);
    }

    public void SetConsoleCanvasActive(bool isActive)
    {
        consoleCanvas.SetActive(isActive);
    }
}
