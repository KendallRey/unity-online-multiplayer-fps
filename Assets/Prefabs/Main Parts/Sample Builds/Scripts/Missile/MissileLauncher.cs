using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System;

public class MissileLauncher : MonoBehaviour
{

    #region Private Variables
    Animator LauncherAnim;
    bool isOpen = false;
    bool isLoaded = false;
    MissileBase missile;
    Transform _tM;
    CancellationTokenSource source;
    #endregion

    #region Getter & Setter
    public MissileBase Missile { set {  missile = value; } }
    public bool Isloaded { 
        get { return isLoaded; }
        set { isLoaded = value; } }
    public Transform Target
    {
        get { return _tM; }
        set { _tM = value; }
    }
    #endregion
    private void Awake()
    {
        ComponentsInit();
    }

    public void LoadMissileNoAsync(MissileBase missile)
    {
        if (Isloaded) return;
        Missile = missile;
        Isloaded = true;
    }
    public void FireMissile()
    {
        if (Target == null)
            source.Cancel();
        missile.Target = Target;
        missile.FireMissile();
        Missile = null;
        Isloaded = false;
        CloseLauncher();
    }
    public async Task FireMissileTask()
    {
        OpenLauncher();
        try
        { await Task.Delay(1200, source.Token); }
        catch (TaskCanceledException) {
            CloseLauncher();
            return; 
        }
        if (Target == null)
            source.Cancel();
        missile.Target = Target;
        missile.FireMissile();
        Missile = null;
        Isloaded = false;
        CloseLauncher();
    }

    void ComponentsInit()
    {
        LauncherAnim = GetComponent<Animator>();
        source = new CancellationTokenSource();
    }

    public void OpenLauncher()
    {
        isOpen = true;
        LauncherAnim.SetBool("isOpen", isOpen);
    }
    private async void CloseLauncher()
    {
        try
        { await Task.Delay(2000, source.Token); }
        catch (TaskCanceledException)
        { return; }
        isOpen = false;
        LauncherAnim.SetBool("isOpen", isOpen); 
    }

    private void OnDestroy()
    {
        source.Cancel();
    }

}
