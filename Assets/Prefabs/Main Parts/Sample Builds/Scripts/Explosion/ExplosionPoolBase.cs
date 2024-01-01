using UnityEngine;
using UnityEngine.Pool;

public abstract class ExplosionPoolBase : MonoBehaviour, IPool<ExplosionBase>
{
    [Header("Pool Settings")]
    [SerializeField] protected static string PoolName = "ExplosionStaticPool";
    [SerializeField] private int DefaultCapacity = 30;
    [SerializeField] private int MaxCapacity = 300;
    [SerializeField] private ExplosionBase ExplosionPrefab;

    private ObjectPool<ExplosionBase> pool;

    public ObjectPool<ExplosionBase> Pool { 
        get { return pool; } 
        private set { pool = value; } 
    }

    public ExplosionBase Prefab => ExplosionPrefab;

    public ExplosionBase CreateFromPool()
    {
        var explosion = Instantiate(Prefab);
        explosion.Initialize(Pool);
        return explosion;
    }

    public void CreatePool()
    {
        Pool = new ObjectPool<ExplosionBase>(
            CreateFromPool,
            GetFromPool,
            ReleaseFromPool,
            DestroyFromPool,
            false,
            DefaultCapacity,
            MaxCapacity);
    }

    public void DestroyFromPool(ExplosionBase obj)
    {
        Destroy(obj.gameObject);
    }

    public void GetFromPool(ExplosionBase obj)
    {
        obj.gameObject.SetActive(true);
    }
    public void Spawn(Transform location)
    {
        ExplosionSet(Pool.Get(), location);
    }
    private ExplosionBase ExplosionSet(ExplosionBase explosion, Transform location)
    {
        var _t = explosion.transform;
        _t.position = location.position;
        explosion.Explode();
        return explosion;
    }

    public void ReleaseFromPool(ExplosionBase obj)
    {
        obj.gameObject.SetActive(false);
    }
}
