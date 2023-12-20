using Photon.Pun;
using UnityEngine;

public abstract class PlayerView : MonoBehaviour
{
    [SerializeField] protected PhotonView view;

    private void Awake()
    {
        if(view == null)
            view = GetComponent<PhotonView>();
    }
}
