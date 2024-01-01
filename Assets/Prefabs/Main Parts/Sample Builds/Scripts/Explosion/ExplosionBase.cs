using UnityEngine;
using UnityEngine.Pool;

public abstract class ExplosionBase : MonoBehaviour, IExplosion
{
    public float Duration { get; set; }
    public ParticleSystem Particle { get; set; }
    public ObjectPool<ExplosionBase> Pool { get; set; }

    public void Initialize(ObjectPool<ExplosionBase> pool)
    {
        Pool = pool;
        Particle = GetComponent<ParticleSystem>();
        Duration = Particle.main.duration;
    }
    public void Explode()
    {
        Particle.Play();
        Invoke(nameof(ReleaseFromPool), Duration);
    }
    public void ReleaseFromPool()
    {
        Pool.Release(this);
    }

}
