using UnityEngine;

public class MissileMediumPool : MissilePoolBase
{
    private static MissileMediumPool instance;
    public static MissileMediumPool Instance
    {
        get
        {
            if (instance == null)
            { instance = new GameObject(PoolName).AddComponent<MissileMediumPool>(); }
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
