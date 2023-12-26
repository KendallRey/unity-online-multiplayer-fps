using UnityEngine;
using TMPro;
using Photon.Pun;

public class ConsoleManager : MonoBehaviour
{
    #region TEST CONSOLE COMMANDS
    /* YEAH, YOU CAN PUT CHEATS HERE*/

    #endregion


    [SerializeField] TMP_InputField consoleInput;
    [SerializeField] GameObject consoleCanvas;
    [SerializeField] GameObject consoleItemPrefab;
    [SerializeField] Transform container;
    [SerializeField] PhotonView PV;

    public TMP_InputField ConsoleInput { get => consoleInput; private set => consoleInput = value; }

    public void SetConsoleCanvasActive(bool isActive)
    {
        consoleCanvas.SetActive(isActive);
    }

    public ConsoleItem AddConsoleItem(string text)
    {
        ConsoleItem consoleItem = Instantiate(consoleItemPrefab, container).GetComponent<ConsoleItem>();
        consoleItem.CommandText.text = text;
        return consoleItem;
    }
}
