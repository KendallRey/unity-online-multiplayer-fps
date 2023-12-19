using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform spawnPosition;
    void Start()
    {
        if(PhotonNetwork.IsConnectedAndReady)
            SpawnPlayerOnline();
        else
            SpawnPlayer();
    }

    void SpawnPlayerOnline()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, Quaternion.identity);
        player.TryGetComponent(out PlayerController playerController);
        if (playerController == null) return;
        playerController.SetSpawner(this);
    }
    void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab);
        player.TryGetComponent(out PlayerController playerController);

        playerController.SetSpawner(this);
        player.transform.position = spawnPosition.transform.position;
    }

    public Vector3 GetRandomPosition()
    {
        return spawnPosition.position;
    }
}
