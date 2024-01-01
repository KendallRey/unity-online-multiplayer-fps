using UnityEngine;
using UnityEngine.Pool;

public class ExplosionPool : ExplosionPoolBase
{

    private static ExplosionPool instance;
    public static ExplosionPool Instance
    {
        get
        {
            if (instance == null)
            { instance = new GameObject(PoolName).AddComponent<ExplosionPool>(); }
            return instance;
        }
        private set { instance = value; }
    }

    private void Awake()
    {
        Instance = this;
        CreatePool();
    }

}
