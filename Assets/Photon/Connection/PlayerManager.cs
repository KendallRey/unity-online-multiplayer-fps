using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
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
        player.TryGetComponent(out HealthManager healthManager);
        if (healthManager == null) return;
        healthManager.PlayerManager = this;
    }
    void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = spawnPosition.position;
    }

    public void RespawnPlayerOnline(GameObject obj)
    {
        PhotonNetwork.Destroy(obj);
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, Quaternion.identity);
        player.TryGetComponent(out HealthManager healthManager);
        if (healthManager == null) return;
        healthManager.PlayerManager = this;
    }

    [SerializeField] Transform[] spawnPositions1;
    [SerializeField] Transform[] spawnPositions2;
    public Vector3 GetRandomPosition(int group)
    {
        Vector3 position;
        if (spawnPositions1.Length == 0 && spawnPositions2.Length == 0)
            return spawnPosition.position;
        switch (group)
        {
            case 1:
                if (spawnPositions1.Length == 0)
                    position = spawnPosition.position;
                else
                {
                    int index = Random.Range(0, spawnPositions1.Length - 1);
                    return spawnPositions1[index].position;
                }
                break;
            case 2:
                if (spawnPositions2.Length == 0)
                    position = spawnPosition.position;
                else
                {
                    int index = Random.Range(0, spawnPositions2.Length - 1);
                    return spawnPositions2[index].position;
                }
                break;
            default:
                position = spawnPosition.position;
                break;
        }

        return position;
    }
}
