using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
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
		}
		else
		{
			Instance = this;
		}
	}

    private void Start()
	{
		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
	}

    private void OnTriggerEnter(Collider other)
	{
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