using UnityEngine;
using UnityEngine.Pool;
public class FXItem : MonoBehaviour
{
    FXPooler fxPooler;
    public FXPooler FXPooler { get => fxPooler; set => fxPooler = value; }

    [SerializeField] ParticleSystem particle;

    private void OnEnable()
    {
        particle.Play();
        Invoke(nameof(OnEnd), 3f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public void OnEnd()
    {
        if (FXPooler != null)
            FXPooler.ReleaseFXItem(this);
    }
}
