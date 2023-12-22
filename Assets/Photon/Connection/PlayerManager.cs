using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
    public PhotonView PV;
    [SerializeField] GameObject playerPrefab;

    int kills, deaths;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        SpawnPlayerOnline();
    }

    void SpawnPlayerOnline()
    {
        if (PV.IsMine)
        {
            Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
            GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), spawnpoint.position, Quaternion.identity, 0, new object[] { PV.ViewID });
            PlayerView[] playerViews = player.GetComponents<PlayerView>();
            foreach(PlayerView view in playerViews)
            {
                view.PlayerManager = this;
            }
        }
    }
    void SpawnPlayer()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = spawnpoint.position;
    }

    public void RespawnPlayerOnline(GameObject obj)
    {
        PhotonNetwork.Destroy(obj);
        SpawnPlayerOnline();
    }

    [SerializeField] Transform[] spawnPositions1;
    [SerializeField] Transform[] spawnPositions2;
    public Vector3 GetRandomPosition(int group)
    {
        Vector3 position;
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        if (spawnPositions1.Length == 0 && spawnPositions2.Length == 0)
        {
            return spawnpoint.position;
        }
        switch (group)
        {
            case 1:
                if (spawnPositions1.Length == 0)
                    position = spawnpoint.position;
                else
                {
                    int index = Random.Range(0, spawnPositions1.Length - 1);
                    return spawnPositions1[index].position;
                }
                break;
            case 2:
                if (spawnPositions2.Length == 0)
                    position = spawnpoint.position;
                else
                {
                    int index = Random.Range(0, spawnPositions2.Length - 1);
                    return spawnPositions2[index].position;
                }
                break;
            default:
                position = spawnpoint.position;
                break;
        }

        return position;
    }
    public void Die()
    {
        deaths++;
        Hashtable hash = new Hashtable();
        hash.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public void GetKill()
    {
        PV.RPC(nameof(RPC_GetKill), PV.Owner);
    }

    [PunRPC]
    public void RPC_GetKill()
    {
        kills++;
        Hashtable hash = new Hashtable();
        hash.Add("kills", kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public static PlayerManager Find(Player player)
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);
    }
}
