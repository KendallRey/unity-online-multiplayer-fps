using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameTimer : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] float gameTimeInSeconds = 300f;
    [SerializeField] TextMeshProUGUI timerText;
    private float timer;

    void Start()
    {
        PhotonNetwork.SendRate = 30; // Adjust the send rate as needed
        PhotonNetwork.SerializationRate = 30; // Adjust the serialization rate as needed

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
            stream.SendNext(timer);
        }
        else
        {
            timerText.text = timer.ToString();
            timer = (float)stream.ReceiveNext();
        }
    }
}
