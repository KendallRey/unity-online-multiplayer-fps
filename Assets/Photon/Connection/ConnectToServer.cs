using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using System.Collections;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] TextMeshProUGUI cueText;
    private bool canProceed = false;

    [SerializeField] string lobbyScene;

    void Start()
    {
        DontDestroyOnLoad(this);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(LoadLobbyScene());
    }

    private IEnumerator LoadLobbyScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(lobbyScene);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            loadingText.text = "Loading: " + (progress * 100f).ToString("F0") + "%";

            if (progress >= 0.9f)
            {
                loadingText.text = "DONE";
                cueText.enabled = true;
                canProceed = true;
            }

            yield return null;
        }
    }

    private void Update()
    {
        if (canProceed && Input.GetKeyDown(KeyCode.Space))
        {
            canProceed = false;
            OnFinishLoading();
            SceneManager.LoadScene(lobbyScene);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnFinishLoading()
    {
        StopAllCoroutines();
        Debug.Log("Loading Complete!");
    }
}
