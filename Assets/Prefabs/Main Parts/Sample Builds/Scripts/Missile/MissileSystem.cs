using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class MissileSystem : MonoBehaviour
{
    [Header("Components"), Space]
    [SerializeField] MissileLauncher[] MissileLaunchers;
    [SerializeField] MissilePoolBase MissilePool;

    #region Override Launchers
    [Header("Override Launchers"), Space]
    [SerializeField] int launchInterval = 300;
    [SerializeField] int loadingTime = 3000;
    [SerializeField] bool StartFiring = true;
    [SerializeField] float openInterval = 0.2f;
    [SerializeField] Transform target;

    #endregion


    #region Private Variables
    [SerializeField] private MissileBase[] missileHolder;
    private int missileCount = 0;
    private CancellationTokenSource source;
    
    #endregion


    private void Awake()
    {
        Init();
        SetLaunchers();
    }

    void SetLaunchers()
    {
        if (MissileLaunchers.Length != 0) return;
        MissileLaunchers = GetComponentsInChildren<MissileLauncher>();
        missileCount = MissileLaunchers.Length;
    }

    void Init()
    {
        source = new CancellationTokenSource();
    }
    void Start()
    {
        LoadMissile();
    }

    //Using =====
    #region Coroutine Setup
    void LoadMissile()
    {
        for (int c = 0; c < missileCount; c++)
        {
            var _t = MissileLaunchers[c].transform;
            MissileLaunchers[c].LoadMissileNoAsync(MissilePool.Spawn(_t));
        }
        StartCoroutine(nameof(Openlaunchers));
    }
    IEnumerator Loadlaunchers()
    {
        foreach (MissileLauncher launcher in MissileLaunchers)
        {
            var _t = launcher.transform;
            launcher.LoadMissileNoAsync(MissilePool.Spawn(_t));
            yield return new WaitForSeconds(openInterval);
        }
        StartCoroutine(nameof(Openlaunchers));
    }
    IEnumerator Openlaunchers()
    {
        foreach (MissileLauncher launcher in MissileLaunchers)
        {
            launcher.OpenLauncher();
            yield return new WaitForSeconds(openInterval);
        }
        StartCoroutine(nameof(Firelaunchers));
    }
    IEnumerator Firelaunchers()
    {
        foreach (MissileLauncher launcher in MissileLaunchers)
        {
            launcher.Target = GetTarget();
            launcher.FireMissile();
            yield return new WaitForSeconds(0.4f);
        }
        if (StartFiring)
        { StartCoroutine(Loadlaunchers()); }
    }

    #endregion
    //Using =====

    #region Async Setup
    async void SetupLaunchers()
    {
        if (!StartFiring)
            return;

        while (!source.IsCancellationRequested)
        {
            try
            {
                await LoadMissiles();
                await FireMissiles();
            }
            catch (TaskCanceledException) { return; }
        }
    }
    async Task FireMissiles()
    {
        var fireTask = new List<Task>();
        foreach (MissileLauncher launcher in MissileLaunchers)
        {
            launcher.Target = GetTarget();
            fireTask.Add(launcher.FireMissileTask());
            await Task.Delay(launchInterval);
        }
        await Task.WhenAll(fireTask);
    }
    async Task LoadMissiles()
    {
        await Task.Delay(loadingTime, source.Token);
        for (int c=0; c < missileCount; c++)
        {
            if (source.IsCancellationRequested) return;
            var _t = MissileLaunchers[c].transform;
            MissileLaunchers[c].LoadMissileNoAsync(MissilePool.Spawn(_t));
        }
        foreach (MissileLauncher launcher in MissileLaunchers)
        {
            await Task.Delay(launchInterval / 2, source.Token);
            launcher.OpenLauncher();
        }
    }

    #endregion

    private Transform GetTarget()
    {
        int index = Random.Range(0, target.childCount);
        var childTarget = target.GetChild(index);
        return childTarget;
    }

    private void OnDestroy()
    {
        source.Cancel();
    }
}