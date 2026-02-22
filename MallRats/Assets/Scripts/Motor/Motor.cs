using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Motor : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField]
    private CurveProfile _curveProfile;
    public CurveProfile CurveProfile
    {
        get { return _curveProfile; }
    }

    private FrictionCalculator _frictionCalculator;
    private float _frictionToApply;
    private float _velocityDirectionScalar; 
    private Vector3 _targetAcceleration;

    protected void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _frictionCalculator = new FrictionCalculator(_rigidbody);
    }

    protected void Reset()
    {
        _curveProfile = ScriptableObject.CreateInstance<CurveProfile>();
    }

    public void Move(Vector3 direction, float force, float friction)
    {
        // Gets the difference in direction between our current velocity and the target.
        _velocityDirectionScalar = Vector3.Dot(direction, _rigidbody.linearVelocity.normalized);

        // Multiplies the desired friction onto the CurveProfile's time curve, giving us a small scalar value to apply.
        // Super useful with changing directions while moving and not feeling like the ground is ice.
        _frictionToApply = friction * _curveProfile.Curve.Evaluate(_velocityDirectionScalar);

        // Apply the friction we evaluated from the CurveProfile above to our friction calculator..
        _targetAcceleration = _frictionCalculator.CalculateFrictionForce(direction, force, _frictionToApply, Time.fixedDeltaTime);

        // Apply the claculated force to the rigidbody.
        _rigidbody.AddForce(_targetAcceleration, ForceMode.Force);
    }
}
