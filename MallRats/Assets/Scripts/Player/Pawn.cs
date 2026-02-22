using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Pawn : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody
    {
        get { return _rigidbody; }
    }

    private CapsuleCollider _collider;
    public CapsuleCollider Collider
    {
        get { return _collider; }
    }

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider  = GetComponent<CapsuleCollider>();
    }

    protected virtual void Reset()
    {
        _rigidbody                  = GetComponent<Rigidbody>();
        _rigidbody.useGravity       = false;
        _rigidbody.interpolation    = RigidbodyInterpolation.None;
        _rigidbody.constraints      = RigidbodyConstraints.FreezeRotation;
        _rigidbody.linearDamping    = 0.0f;
        _rigidbody.angularDamping   = 0.0f;

        _collider           = GetComponent<CapsuleCollider>();
        _collider.height    = 2.0f;
        _collider.radius    = 0.3f;
        _collider.center    = new Vector3(0.0f, 0.95f, 0.0f);   // This makes it so the raycast for the feet are slightly in the collider.
        _collider.material  = new PhysicsMaterial("Frictionless Physics Material")
        {
            frictionCombine = PhysicsMaterialCombine.Minimum,
            staticFriction  = 0.0f,
            dynamicFriction = 0.0f
        };
    }
}