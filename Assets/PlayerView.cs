using Photon.Pun;
using UnityEngine;

public abstract class PlayerView : MonoBehaviour
{
    [SerializeField] protected PhotonView view;
    protected PlayerManager playerManager;
    public PlayerManager PlayerManager { get => playerManager; set => playerManager = value; }

    private void Awake()
    {
        if(view == null)
            view = GetComponent<PhotonView>();
    }
}
