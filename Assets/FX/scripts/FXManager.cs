using UnityEngine;
using Photon.Pun;

public class FXManager : MonoBehaviour
{
	public static FXManager Instance;

	const string WOOD = "Wood";
	const string METAL = "Metal";
	const string CONCRETE = "Concrete";
	const string FLESH = "Flesh";

	[SerializeField] FXPooler bulletHitWoodPooler;
	[SerializeField] FXPooler bulletHitMetalPooler;
	[SerializeField] FXPooler bulletHitConcretePooler;
	[SerializeField] FXPooler bulletHitFleshPooler;

	[SerializeField] PhotonView PV;
	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
	public void OnBulletHitFX(Collider collider, Vector3 position, Vector3 origin)
	{
		Vector3 direction = origin - position;
		Quaternion rotation = Quaternion.LookRotation(direction);
		OnGetFXPooler(collider.tag, position, rotation);
		PV.RPC(nameof(RPC_OnBulletHitFX), RpcTarget.Others, collider.tag, position, rotation);
	}


	[PunRPC]
	public void RPC_OnBulletHitFX(string type, Vector3 position, Quaternion rotation)
	{
		if (PV.IsMine) return;
		OnGetFXPooler(type, position, rotation);
	}

	private void OnGetFXPooler(string type, Vector3 position, Quaternion rotation)
	{
		switch (type)
		{
			case CONCRETE:
				bulletHitConcretePooler.GetFXItem(position, rotation);
				break;
			case WOOD:
				bulletHitWoodPooler.GetFXItem(position, rotation);
				break;
			case METAL:
				bulletHitMetalPooler.GetFXItem(position, rotation);
				break;
			case FLESH:
				bulletHitFleshPooler.GetFXItem(position, rotation);
				break;
		}
	}
}
