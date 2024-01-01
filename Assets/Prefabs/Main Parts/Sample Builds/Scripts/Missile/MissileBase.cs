using UnityEngine;
using UnityEngine.Pool;

public abstract class MissileBase : MonoBehaviour, IMissile
{
    [Header("Components")]
    [SerializeField] Rigidbody missileRB;
    [SerializeField] Collider missileCollider;
    [Header("Missile Config")]
    [SerializeField] float Speed = 50;
    [SerializeField] int LifeSpan = 30,
        trackRate = 30,
        proximityFuse = 5,
        timeArmed = 3,
        timeTrack = 4,
        leadTime = 1;

    #region Private Variables
    bool isTracking = false;
    bool isArmed = false;
    float distance;
    Vector3 offset, finalOffset;
    Rigidbody targetRB;
    Transform _t, target;
    Quaternion rotation;
    ObjectPool<MissileBase> Pool;
    #endregion

    #region Getter & Setter
    public Transform Target { 
        get => target; 
        set {
            target = value;
            TargetRB = target.GetComponentInParent<Rigidbody>();
        } }
    public Rigidbody TargetRB { get => targetRB; set => targetRB = value; }
    #endregion

    private void Awake()
    {
        _t = transform;
    }
    private void OnEnable()
    {
        Invoke(nameof(Detonate), LifeSpan);
        Invoke(nameof(SetArmed), timeArmed);
        Invoke(nameof(SetTracking), timeTrack);
    }
    public void Initialize(ObjectPool<MissileBase> pool)
    {
        Pool = pool;
    }
    void SetArmed()
    {
        missileCollider.enabled = true;
        isArmed = true;
    }

    void SetTracking()
    {
        isTracking = true;
    }

    bool destroyed = false;
    public bool Destroyed
    {
        get { return destroyed; }
        set { destroyed = value; }
    }
    public void FireMissile()
    {
        Destroyed = false;
        SetObject(true);
        if(_t == null) { return; }
        _t.SetParent(null);
    }
    public void InitComponents()
    {
        TargetRB = Target.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isTracking) return;
        if (TargetRB) finalOffset = PredictMovement(leadTime);
        else finalOffset = Target.position;
        TrackTarget(finalOffset);
        if (!TargetInProx()) return;
        if (destroyed) return;
            Detonate();
    }

    void TrackTarget(Vector3 deviatedPrediction)
    {
        offset = deviatedPrediction - _t.position;
        rotation = Quaternion.LookRotation(offset);
        missileRB.MoveRotation(Quaternion.RotateTowards(_t.rotation, rotation, trackRate * Time.deltaTime));
    }

    private Vector3 PredictMovement(float leadTime)
    {
        var predictionTime = Mathf.Lerp(0, 10, leadTime);
        var basePrediction = Target.position + TargetRB.velocity * predictionTime;
        return basePrediction;
    }

    private void FixedUpdate()
    {
        missileRB.velocity = _t.forward * Speed;
    }
    private bool TargetInProx()
    {
        distance = Vector3.Distance(_t.position, Target.position);
        return (distance <= proximityFuse);
    }

    private void Detonate()
    {
        Destroyed = true;
        CancelInvoke();
        SetObject(false);
        ExplosionPool.Instance.Spawn(_t);
        ReleaseMissile();
    }

    private void ReleaseMissile()
    {
        Pool.Release(this);
    }

    private void SetObject(bool isEnabled)
    {
        if (this == null) return;
        enabled = isEnabled;
        isArmed = false;
        isTracking = false;
        missileCollider.enabled = false;
        missileRB.isKinematic = !isEnabled;
    }

    private void OnCollisionEnter()
    {
        if (destroyed) return;
        Detonate();
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
