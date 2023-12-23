using UnityEngine;
using Photon.Pun;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
	public static RoomManager Instance;
	[SerializeField] Scoreboard scoreboard;
	public Scoreboard Scoreboard { get => scoreboard; private set => scoreboard = value; }

	[SerializeField] ConsoleManager consoleManager;
	public ConsoleManager ConsoleManager { get => consoleManager; private set => consoleManager = value; }
	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			Debug.Log("Room Manager already exists, this is destroyed!");
		}
		else
		{
			Instance = this;
			Debug.Log("New Room Manager Instantiated.");
			DontDestroyOnLoad(gameObject);
		}
	}

    private void Start()
	{
		Debug.Log("Room Manager Started.");
		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
	}

    private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Entered : " + other.name);
		if (other.TryGetComponent(out HealthManager healthManager))
        {
			healthManager.Die();
        }
    }
    public void ExitGame()
    {
		// Check if the application is running in the Unity Editor
		#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
		#else
				// Quit the application when not in the Unity Editor
				Application.Quit();
		#endif
	}
}