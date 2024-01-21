using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;
    [SerializeField] CanvasGroup canvasGroup;

    Dictionary<Player, ScoreboardItem> scoreboardItems = new Dictionary<Player, ScoreboardItem>();

    bool isScoreboardOpen = false;

    private void Start()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            AddScoreboardItem(player);
        }
    }

    public KeyValuePair<Player, ScoreboardItem> GetWinnerPlayer()
    {
        return GetPlayerWithHighestKills(scoreboardItems);
    }

    static KeyValuePair<Player, ScoreboardItem> GetPlayerWithHighestKills(Dictionary<Player, ScoreboardItem> scoreboardItems)
    {
        KeyValuePair<Player, ScoreboardItem> playerWithHighestKills = scoreboardItems.OrderByDescending(kv => kv.Value.GetScoreBoardKills())
                                                                                      .FirstOrDefault();

        return playerWithHighestKills;
    }

    static KeyValuePair<Player, ScoreboardItem> GetPlayerWithBestStats(Dictionary<Player, ScoreboardItem> scoreboardItems)
    {
        // Use LINQ to order the items by kills in descending order and then by deaths in ascending order
        KeyValuePair<Player, ScoreboardItem> playerWithBestStats = scoreboardItems.OrderByDescending(kv => kv.Value.GetScoreBoardKills())
                                                                                      .ThenBy(kv => kv.Value.GetScoreBoardDeaths())
                                                                                      .FirstOrDefault();

        return playerWithBestStats;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreboardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreboardItem(otherPlayer);
    }
    void AddScoreboardItem(Player player)
    {
        ScoreboardItem item = Instantiate(scoreboardItemPrefab, container).GetComponent<ScoreboardItem>();
        item.Initialize(player);
        scoreboardItems[player] = item;
    }

    void RemoveScoreboardItem(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }

    public void OnViewScoreboard()
    {
        isScoreboardOpen = !isScoreboardOpen;
        if (isScoreboardOpen)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            canvasGroup.alpha = 0f;
        }
    }
}
