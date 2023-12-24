using UnityEngine;
using UnityEngine.Pool;
using Photon.Pun;

public class SFXManager : MonoBehaviour
{

	const string WOOD = "Wood";
	const string METAL = "Metal";
	const string CONCRETE = "Concrete";
	const string FLESH = "Flesh";

	[SerializeField] SFXPooler bulletHitWoodPooler;
	[SerializeField] SFXPooler bulletHitMetalPooler;
	[SerializeField] SFXPooler bulletHitConcretePooler;
	[SerializeField] SFXPooler bulletHitFleshPooler;
	[SerializeField] SFXPooler bulletShootPooler;

	[SerializeField] PhotonView pv;
	public PhotonView PV { get => pv; set => pv = value; }
	public void OnFireSFX(Vector3 position)
	{
		bulletShootPooler.GetSFXItem(position);
		PV.RPC(nameof(RPC_OnFire), RpcTarget.Others, position);
	}
	[PunRPC]
	public void RPC_OnFire(Vector3 position)
	{
		if (PV.IsMine) return;
		bulletShootPooler.GetSFXItem(position);
	}

	public void OnBulletHitSFX(Collider collider, Vector3 position)
    {
		OnGetSFXPooler(collider.tag, position);
		PV.RPC(nameof(RPC_OnBulletHit), RpcTarget.Others, collider.tag, position);
	}


	[PunRPC]
	public void RPC_OnBulletHit(string type, Vector3 position)
	{
		if (PV.IsMine) return;
		OnGetSFXPooler(type, position);
	}

	private void OnGetSFXPooler(string type, Vector3 position)
    {
		switch (type)
		{
			case CONCRETE:
				bulletHitConcretePooler.GetSFXItem(position);
				break;
			case WOOD:
				bulletHitWoodPooler.GetSFXItem(position);
				break;
			case METAL:
				bulletHitMetalPooler.GetSFXItem(position);
				break;
			case FLESH:
				bulletHitFleshPooler.GetSFXItem(position);
				break;
		}
	}
}
