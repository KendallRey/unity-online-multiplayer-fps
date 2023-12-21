using UnityEngine;
using TMPro;

public class PlayerNameDisplay : PlayerView
{
    [SerializeField] TextMeshProUGUI playerName; 
    void Start()
    {
        playerName.text = view.Owner.NickName;
    }
}
