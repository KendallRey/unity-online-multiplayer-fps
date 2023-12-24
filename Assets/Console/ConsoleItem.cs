using TMPro;
using UnityEngine;

public class ConsoleItem : MonoBehaviour
{
    [SerializeField] TMP_Text commandText;
    public TMP_Text CommandText { get => commandText; set => commandText = value; }
}
