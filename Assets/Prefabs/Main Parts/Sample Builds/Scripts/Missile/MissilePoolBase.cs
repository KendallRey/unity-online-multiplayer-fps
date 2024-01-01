using UnityEngine;
using UnityEngine.Pool;

public abstract class MissilePoolBase : MonoBehaviour, IPool<MissileBase>
{
    [Header("Pool Settings")]
    [SerializeField] protected static string PoolName = "MissileStaticPool";
    [SerializeField] private int DefaultCapacity = 50;
    [SerializeField] private int MaxCapacity = 200;
    [SerializeField] protected MissileBase MissilePrefab;
    public MissileBase Prefab => MissilePrefab;

    private ObjectPool<MissileBase> pool;
    public ObjectPool<MissileBase> Pool
    {
        get { return pool; }
        private set { pool = value; }
    }
    public MissileBase CreateFromPool()
    {
        var missile = Instantiate(Prefab);
        missile.Initialize(Pool);
        return missile;
    }

    public void CreatePool()
    {
        Pool = new ObjectPool<MissileBase>(
             CreateFromPool,
             GetFromPool,
             ReleaseFromPool,
             DestroyFromPool,
             false,
             DefaultCapacity,
             MaxCapacity);
    }

    public void DestroyFromPool(MissileBase obj)
    {
        Destroy(obj.gameObject);
    }

    public void GetFromPool(MissileBase obj)
    {
        obj.gameObject.SetActive(true);
    }
    public MissileBase Spawn(Transform location)
    {
        return MissileSet(Pool.Get(), location);
    }
    private MissileBase MissileSet(MissileBase missile, Transform parent)
    {
        var _t = missile.transform;
        _t.SetParent(parent);
        _t.localPosition = Vector3.zero;
        _t.localRotation = Quaternion.identity;
        return missile;
    }
    public void ReleaseFromPool(MissileBase obj)
    {
        obj.gameObject.SetActive(false);
    }
}
