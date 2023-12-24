using UnityEngine.Pool;
using UnityEngine;

public class SFXPooler : MonoBehaviour
{
    ObjectPool<SFXItem> sfxPool;
    
    public ObjectPool<SFXItem> SFXPool { get => sfxPool; private set => sfxPool = value; }

    [SerializeField] SFXItem[] sfxPrefabs;
    private void Awake()
    {
        if (sfxPrefabs.Length == 0) return;
        SFXPool = new ObjectPool<SFXItem>(
            CreateSFX,
            GetSFX,
            ReleaseSFX,
            DestroySFX,
            false,
            100,
            500
            );
    }

    SFXItem CreateSFX()
    {
        int randomIndex = Random.Range(0, sfxPrefabs.Length);
        SFXItem item = Instantiate(sfxPrefabs[randomIndex], transform);
        item.SFXPooler = this;
        return item;
    }

    void GetSFX(SFXItem item)
    {
        item.SFXPooler = this;
        item.gameObject.SetActive(true);
    }
    void ReleaseSFX(SFXItem item)
    {
        item.gameObject.SetActive(false);
    }
    void DestroySFX(SFXItem item)
    {
        Destroy(item.gameObject);
    }

    public void GetSFXItem(Vector3 position)
    {
        SFXItem sfxItem = SFXPool.Get();
        sfxItem.transform.position = position;
    }
    
    public void ReleaseSFXItem(SFXItem item)
    {
        SFXPool.Release(item);
    }
}
