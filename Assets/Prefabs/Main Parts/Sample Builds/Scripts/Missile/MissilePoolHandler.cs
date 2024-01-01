using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class MissilePoolHandler : MonoBehaviour
{
    [SerializeField] int defaultCapacity = 100;
    [SerializeField] int maxCapacity = 200;

    private static MissilePoolHandler instance;
    public static MissilePoolHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("MissilePoolStatics").AddComponent<MissilePoolHandler>();
            }
            return instance;
        }
    }

    private MissileBase missilePrefabS1;
    private MissileBase missilePrefabM1;
    public MissileBase MissilePrefabS1 {
        set { missilePrefabS1 = value; } 
        get { return missilePrefabS1; }
    }
    public MissileBase MissilePrefabM1 {
        set { missilePrefabM1 = value; }
        get { return missilePrefabM1; }
    }

    private ObjectPool<MissileBase> missilePoolS1;
    public ObjectPool<MissileBase> MissilePoolS1
    {
        get { return missilePoolS1; }
    }

    private ObjectPool<MissileBase> missilePoolM1;
    public ObjectPool<MissileBase> MissilePoolM1
    {
        get { return missilePoolM1; }
    }

    public ObjectPool<MissileBase> MissilePool
    {
        get { return missilePoolS1; }
    }


    private void OnEnable()
    {
        InitPoolS1();
        InitPoolM1();
    }
    MissileBase GetSmall()
    {
        return Instantiate(missilePrefabS1);
    }
    MissileBase GetMedium()
    {
        return Instantiate(missilePrefabM1);
    }
    void Take(MissileBase obj)
    {
        obj.gameObject.SetActive(true);
    }
    void Release(MissileBase obj)
    {
        obj.gameObject.SetActive(false);
    }
    void DestroyObj(MissileBase obj)
    {
    }
    private void InitPoolS1()
    {
        missilePoolS1 = new ObjectPool<MissileBase>(
            GetSmall, Take, Release, DestroyObj, false, defaultCapacity, maxCapacity);
    }
    private void InitPoolM1()
    {
        missilePoolM1 = new ObjectPool<MissileBase>(
            GetMedium, Take, Release, DestroyObj, false, defaultCapacity, maxCapacity);;
    }

    private MissileBase MissileSetter(MissileBase missile, Transform parent)
    {
        var _t = missile.transform;
        _t.SetParent(parent);
        _t.localPosition = Vector3.zero;
        _t.localRotation = Quaternion.identity;
        return missile;
    }
    public MissileBase GetMissileSmall(Transform parent)
    {
        return MissileSetter(missilePoolS1.Get(), parent);
    }
    public MissileBase GetMissileMedium(Transform parent)
    {
        return MissileSetter(missilePoolM1.Get(), parent);
    }
    public void ReleaseMissileSmall(MissileBase missile)
    {
        missilePoolS1.Release(missile);
    }
    public void ReleaseMissileMedium(MissileBase missile)
    {
        missilePoolM1.Release(missile);
    }
    

    private void OnDestroy()
    {
        missilePoolS1.Clear();
        missilePoolM1.Clear();
    }
}
