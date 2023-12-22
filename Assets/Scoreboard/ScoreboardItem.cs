using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreboardItem : MonoBehaviourPunCallbacks
{


    [SerializeField] TMP_Text usernameText;
    [SerializeField] TMP_Text killsText;
    [SerializeField] TMP_Text deathsText;

    Player player;

    public void Initialize(Player player)
    {
        this.player = player;
        usernameText.text = player.NickName;
        UpdateStats();
    }
    void UpdateStats()
    {
        if(player.CustomProperties.TryGetValue("kills", out object kills))
        {
            killsText.text = kills.ToString();
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(targetPlayer == player)
        {
            if (changedProps.ContainsKey("kills"))
            {
                UpdateStats();
            }
        }
    }
}
