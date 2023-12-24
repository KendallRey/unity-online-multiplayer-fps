using UnityEngine;
using UnityEngine.Pool;
using Photon.Pun;

public class SFXManager : MonoBehaviour
{
	public static SFXManager Instance;

	const string WOOD = "Wood";
	const string METAL = "Metal";
	const string CONCRETE = "Concrete";
	const string FLESH = "Flesh";

	[SerializeField] SFXPooler bulletHitWoodPooler;
	[SerializeField] SFXPooler bulletHitMetalPooler;
	[SerializeField] SFXPooler bulletHitConcretePooler;
	[SerializeField] SFXPooler bulletHitFleshPooler;

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
	public void OnFireSFX(PhotonView pv, Vector3 position)
	{
		
	}
	[PunRPC]
	public void RPC_OnFire(PhotonView pv, Vector3 position)
	{
	}

	public void OnBulletHitSFX(PhotonView pv, Collider collider, Vector3 position)
    {
		OnGetSFXPooler(collider.tag, position);
		pv.RPC(nameof(RPC_OnBulletHit), RpcTarget.Others, collider.tag, position);
	}


	[PunRPC]
	public void RPC_OnBulletHit(string type, Vector3 position)
	{
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
