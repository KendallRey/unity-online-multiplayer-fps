using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Range(0, 180f)]
    [SerializeField] float angle = 45f;
    [SerializeField] float gunMaxAngle = 45f;
    [SerializeField] float gunMinAngle = 10f;

    [SerializeField] float maxTurretTurnSpeed = 20f;
    [SerializeField] float maxGunTurnSpeed = 20f;
    [SerializeField] Transform target;

    [SerializeField] Transform turretT, gunHolderT;
    Transform turretHardpoint, gunHardPoint;

    bool outOfRange;
    bool aimed;


    void Start()
    {
        if (!turretT || !gunHolderT)
        {
            enabled = false;
            return;
        }
        turretHardpoint = turretT.parent;
        gunHardPoint = gunHolderT;
    }
    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!turretT || !gunHolderT)
        {
            return;
        }
        float range = 20f;
        float dashLineSize = 2f;
        Vector3 origin = turretT.position;
        turretHardpoint = turretT.parent;
        gunHardPoint = gunHolderT;

        if (!turretHardpoint) return;
        var from = Quaternion.AngleAxis(-angle, turretHardpoint.up) * turretHardpoint.forward;

        Handles.color = new Color(0, 1, 0, .2f);
        Handles.DrawSolidArc(origin, turretT.up, from, angle * 2, range);

        if (!target) return;

        var projection = Vector3.ProjectOnPlane(target.position - turretT.position, turretHardpoint.up);

        // projection line
        Handles.color = Color.white;
        Handles.DrawDottedLine(target.position, turretT.position + projection, dashLineSize);

        // do not draw target indicator when out of angle
        if (Vector3.Angle(turretHardpoint.forward, projection) > angle) return;

        // target line
        Handles.color = Color.red;
        Handles.DrawLine(turretT.position, turretT.position + projection);

        // range line
        Handles.color = Color.green;
        Handles.DrawWireArc(origin, turretT.up, from, angle * 2, projection.magnitude);
        Handles.DrawSolidDisc(turretT.position + projection, turretT.up, .5f);
#endif
    }


    void Update()
    {
        AimTurret(target.position);
        AimGun(target.position);
    }

    void AimTurret(Vector3 targetPoint)
    {

        Vector3 direction = targetPoint - turretT.position;

        direction = Vector3.ProjectOnPlane(direction, turretHardpoint.up);
        float signedAngle = Vector3.SignedAngle(turretHardpoint.forward, direction, turretHardpoint.up);

        outOfRange = false;
        if (Mathf.Abs(signedAngle) > angle)
        {
            outOfRange = true;
            direction = turretHardpoint.rotation * Quaternion.Euler(0, Mathf.Clamp(signedAngle, -angle, angle), 0) *
                        Vector3.forward;
        }

        var rotation = Quaternion.LookRotation(direction, turretHardpoint.up);

        aimed = false;
        if (rotation == turretT.rotation && !outOfRange)
        {
            aimed = true;
        }

        turretT.rotation = Quaternion.RotateTowards(turretT.rotation, rotation, maxTurretTurnSpeed * Time.deltaTime);
    }
    void AimGun(Vector3 targetPoint)
    {

        Vector3 direction = GetTargetPositionOnPlane(gunHardPoint, targetPoint);
        float signedAngle = GetSignedAngleFromAxisToTarget(gunHardPoint.right, gunHardPoint, targetPoint);
        direction = ClampRotationToMaxAngle(signedAngle, gunMinAngle, gunMaxAngle, direction);


        Quaternion rotation = Quaternion.LookRotation(direction, gunHardPoint.up);

        aimed = false;
        if (rotation == gunHolderT.rotation && !outOfRange)
        {
            aimed = true;
        }

        gunHolderT.rotation = Quaternion.RotateTowards(gunHolderT.rotation, rotation, maxGunTurnSpeed * Time.deltaTime);
    }

    Vector3 GetTargetPositionOnPlane(Transform originT, Vector3 targetPoint)
    {
        Vector3 direction = targetPoint - originT.position;
        direction = Vector3.ProjectOnPlane(direction, originT.up);
        return direction;
    }

    float GetSignedAngleFromAxisToTarget(Vector3 axis, Transform originT, Vector3 target)
    {
        Vector3 direction = target - originT.position;
        direction = Vector3.ProjectOnPlane(direction, originT.up);
        float signedAngle = Vector3.SignedAngle(originT.forward, direction, axis);
        return signedAngle;
    }

    Vector3 ClampRotationToMaxAngle(float signedAngle, float minAngle, float maxAngle, Vector3 defaultDirection)
    {
        if (signedAngle > maxAngle || signedAngle < minAngle)
        {
            defaultDirection = gunHardPoint.rotation * Quaternion.Euler(0, Mathf.Clamp(signedAngle, minAngle, maxAngle), 0) *
                        Vector3.forward;
        }
        return defaultDirection;
    }
}
