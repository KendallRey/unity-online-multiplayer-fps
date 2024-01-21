using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Collections.Generic;
using TMPro;

public class GameTimer : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] float gameTimeInSeconds = 300f;
    [SerializeField] TextMeshProUGUI timerText;
    private float timer;

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI winnerNameText;
    [SerializeField] TextMeshProUGUI killsText;

    [SerializeField] Scoreboard scoreboard;
    bool hasGameEnded = false;
    bool hasSynced = false;

    void Start()
    {
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;

        if (PhotonNetwork.IsMasterClient)
        {
            timer = gameTimeInSeconds;
            photonView.RPC(nameof(SyncTimer), RpcTarget.Others, timer);
        }
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                Debug.Log("Game Over");
                timer = 0f;
            }

            photonView.RPC(nameof(SyncTimer), RpcTarget.Others, timer);
        }

        if (hasGameEnded) return;
        if (timer <= 0 && (hasSynced || PhotonNetwork.IsMasterClient))
        {
            hasGameEnded = true;
            EndGame();
        }

        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");
        timerText.text = $"{minutes}:{seconds}";
    }

    [PunRPC]
    void SyncTimer(float syncTime)
    {
        timer = syncTime;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Debug.Log("Sender");
            stream.SendNext(timer);
        }
        else
        {
            Debug.Log("Reciever");
            timerText.text = timer.ToString();
            hasSynced = true;
            timer = (float)stream.ReceiveNext();
        }
    }

    void EndGame()
    {
        Cursor.lockState = CursorLockMode.None;
        PlayerUIManager[] playerUIManagers = FindObjectsOfType<PlayerUIManager>();

        foreach(PlayerUIManager playerUI in playerUIManagers)
        {
            playerUI.OnEndGame();
        }

        KeyValuePair<Player, ScoreboardItem> playerScore = scoreboard.GetWinnerPlayer();

        string playerName = playerScore.Key.NickName;
        int kills = playerScore.Value.GetScoreBoardKills();

        winnerNameText.text = playerName;
        killsText.text = "" + kills + " Kills !!!";

        gameOverPanel.SetActive(true);
    }
}
