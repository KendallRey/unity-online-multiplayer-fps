using UnityEngine;

public class SampleShipContol : MonoBehaviour
{
    [SerializeField] Rigidbody shipRB;
    [SerializeField] Transform _t;
    [SerializeField] float thrusterForce;
    [SerializeField] float stability, stabilitySpeed;
    [SerializeField] float forwardForce;
    [SerializeField] float steerSpeed;
    [SerializeField] float waypointWithinDistance;

    int ID = 0;
    int waypointCount;
    float targetHeight;
    float steerForce = 0;

    [SerializeField] Transform WaypointHolder;
    Transform currentWaypoint;

    void Start()
    {
        Init();
    }

    void Init()
    {
        shipRB = GetComponent<Rigidbody>();
        _t = transform;
        waypointCount = WaypointHolder.childCount;
        SetWaypoint();
    }

    void Update()
    {
        NextWaypoint();
        SetSteer();
    }

    void SetSteer()
    {
        var offset = currentWaypoint.position - _t.position;
        Quaternion degree = Quaternion.LookRotation(offset);
        var angle = degree.eulerAngles.y - _t.eulerAngles.y;
        angle %= 360;
        if (angle > 180) angle -= 360;
        
        steerForce = Mathf.Clamp(angle,-20,20);
    }

    void NextWaypoint()
    {
        if (Vector3.Distance(_t.position, currentWaypoint.position) <= waypointWithinDistance)
        {
            ID++;
            if (ID >= waypointCount) ID = 0;
            SetWaypoint();
        }
    }

    void SetWaypoint()
    {
        currentWaypoint = WaypointHolder.GetChild(ID);
        targetHeight = currentWaypoint.position.y;
    }

    private void FixedUpdate()
    {
        float force = Force();
        shipRB.AddForce(Vector3.up * force, ForceMode.Force);
        shipRB.AddForce(_t.forward * forwardForce, ForceMode.Force);
        shipRB.AddTorque(steerForce * steerSpeed * _t.up, ForceMode.Force);

        Vector3 predictUp = Angle(_t);
        Vector3 torqueVector = Torque(predictUp);
        shipRB.AddTorque(torqueVector * stabilitySpeed * stabilitySpeed);
    }
    private Vector3 Angle(Transform transform)
    {
        float axis = shipRB.angularVelocity.magnitude * Mathf.Rad2Deg * stability / stabilitySpeed;
        return Quaternion.AngleAxis(axis, shipRB.angularVelocity) * transform.up;
    }

    private Vector3 Torque(Vector3 predictUp)
    {
        return Vector3.Cross(predictUp, Vector3.up);
    }
    private float Force()
    {
        var distance = _t.position.y;
        return (targetHeight / distance) * thrusterForce;
    }
}
