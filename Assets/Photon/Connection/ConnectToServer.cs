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
    float progress = 0;
    AsyncOperation asyncOperation;
    private IEnumerator LoadLobbyScene()
    {
        asyncOperation = SceneManager.LoadSceneAsync(lobbyScene);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            OnFinishLoading();
            yield return null;
        }
    }

    private void Update()
    {
        UpdateLoadingText();

        if (canProceed && Input.GetKeyDown(KeyCode.Space))
        {
            canProceed = false;
            SceneManager.LoadScene(lobbyScene);
        }
    }

    void UpdateLoadingText()
    {
        if (asyncOperation == null) return;
        if (!asyncOperation.isDone)
        {
            progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            loadingText.text = "Loading: " + (progress * 100f).ToString("F0") + "%";
        }
        else
        {
            loadingText.text = "DONE";
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnFinishLoading()
    {
        canProceed = true;
        cueText.enabled = true;
        StopAllCoroutines();
        Debug.Log("Loading Complete!");
    }
}
