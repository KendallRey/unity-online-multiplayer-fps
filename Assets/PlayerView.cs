using Photon.Pun;
using UnityEngine;

public abstract class PlayerView : MonoBehaviour
{
    [SerializeField] protected PhotonView view;
    protected PlayerManager playerManager;
    public PlayerManager PlayerManager { get => playerManager; set => playerManager = value; }

    private void Awake()
    {
        playerManager = PhotonView.Find((int)view.InstantiationData[0]).GetComponent<PlayerManager>();
        if(view == null)
            view = GetComponent<PhotonView>();
    }
}
