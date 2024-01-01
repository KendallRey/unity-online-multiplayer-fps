using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMissilePool : MissilePoolBase
{
    private static SmallMissilePool instance;
    public static SmallMissilePool Instance
    {
        get
        {
            if (instance == null)
            { instance = new GameObject(PoolName).AddComponent<SmallMissilePool>(); }
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
