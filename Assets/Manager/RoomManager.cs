using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
	public static RoomManager Instance;

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
}