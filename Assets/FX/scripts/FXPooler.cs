using UnityEngine.Pool;
using UnityEngine;

public class FXPooler : MonoBehaviour
{
    ObjectPool<FXItem> fxPool;

    public ObjectPool<FXItem> FXPool { get => fxPool; private set => fxPool = value; }

    [SerializeField] FXItem[] fxPrefabs;
    private void Awake()
    {
        if (fxPrefabs.Length == 0) return;
        FXPool = new ObjectPool<FXItem>(
            CreateFX,
            GetFX,
            ReleaseFX,
            DestroyFX,
            false,
            200,
            800
            );
    }

    FXItem CreateFX()
    {
        int randomIndex = Random.Range(0, fxPrefabs.Length);
        FXItem item = Instantiate(fxPrefabs[randomIndex], transform);
        item.FXPooler = this;
        return item;
    }

    void GetFX(FXItem item)
    {
        item.FXPooler = this;
        item.gameObject.SetActive(true);
    }
    void ReleaseFX(FXItem item)
    {
        item.gameObject.SetActive(false);
    }
    void DestroyFX(FXItem item)
    {
        Destroy(item.gameObject);
    }

    public void GetFXItem(Vector3 position)
    {
        FXItem fxItem = FXPool.Get();
        fxItem.transform.position = position;
    }
    public void GetFXItem(Vector3 position, Quaternion rotation)
    {
        FXItem fxItem = FXPool.Get();
        fxItem.transform.SetPositionAndRotation(position, rotation);
    }

    public void ReleaseFXItem(FXItem item)
    {
        FXPool.Release(item);
    }
}
