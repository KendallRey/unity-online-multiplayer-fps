using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField nicknameInput;
    string nickname;

    private void Awake()
    {
        string defaultNickname = "Player " + Random.Range(0, 1000).ToString("0000");
        nickname = PlayerPrefs.GetString("nickname", defaultNickname);
        PhotonNetwork.NickName = nickname;
    }

    private void Start()
    {
        nicknameInput.text = nickname;
    }
    public void OnChangeNickname()
    {
        nickname = nicknameInput.text;
        PlayerPrefs.SetString("nickname", nickname);
        PhotonNetwork.NickName = nickname;
    }
}
