
public interface IPool<T>
{
    T Prefab { get; }
    void CreatePool();
    T CreateFromPool();
    void GetFromPool(T obj);
    void ReleaseFromPool(T obj);
    void DestroyFromPool(T obj);

}
