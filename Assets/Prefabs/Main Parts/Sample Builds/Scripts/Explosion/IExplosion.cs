using UnityEngine;
using UnityEngine.Pool;

public interface IExplosion
{
    ObjectPool<ExplosionBase> Pool { get; set; }
    ParticleSystem Particle { get; set; }
    float Duration { get; set; }
    void Initialize(ObjectPool<ExplosionBase> Pool);
    void Explode();
    void ReleaseFromPool();
}
