using UnityEngine;
using UnityEngine.Pool;
public class SFXItem : MonoBehaviour
{
    SFXPooler sfxPooler;
    public SFXPooler SFXPooler { get => sfxPooler; set => sfxPooler = value; }

    [SerializeField] AudioSource audioSource;

    private void OnEnable()
    {
        audioSource.time = 0.14f;
        audioSource.Play();
        Invoke(nameof(OnEnd), 3f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public void OnEnd()
    {
        if(SFXPooler != null)
            SFXPooler.ReleaseSFXItem(this);
    }
}
