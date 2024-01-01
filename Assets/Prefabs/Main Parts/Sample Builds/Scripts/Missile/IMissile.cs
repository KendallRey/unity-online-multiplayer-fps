using UnityEngine;

public interface IMissile
{
    Transform Target { get; set; }
    Rigidbody TargetRB { get; set; }
    void FireMissile();

}
