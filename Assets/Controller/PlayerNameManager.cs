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
        PlayerPrefs.SetString("ConsoleEnabled", "No");
        Debug.Log("Console Commands Disabled by Default");
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
        if (nickname == "ConsoleEnabled")
        {
            Debug.Log("Console Commands Enabled");
            PlayerPrefs.SetString("ConsoleEnabled", "ConsoleEnabled");
        }
    }
}
