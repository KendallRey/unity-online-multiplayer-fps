using UnityEngine;
public class SFXItem : MonoBehaviour
{
    SFXPooler sfxPooler;
    public SFXPooler SFXPooler { get => sfxPooler; set => sfxPooler = value; }

    [SerializeField] AudioSource audioSource;
    [SerializeField] float startTime = 0.14f;
    private void OnEnable()
    {
        audioSource.time = startTime;
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
