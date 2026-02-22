using UnityEngine;

public class FrictionCalculator 
{
    private Rigidbody _rigidbody;

    private Vector3 _currentVelocity;
    private Vector3 _targetVelocity;
    private Vector3 _targetAcceleration;

    public FrictionCalculator(Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
    }

    public Vector3 CalculateFrictionForce(Vector3 direction, float force, float friction, float timeStep)
    {
        _targetVelocity = direction * force;

        // Calculates the position between the current velocity and target velocity by moving 'frictionToApply' units.
        // -- Note this is different from Vector3.Lerp as lerp calculates T as a percentage, where this calculates it as units.
        _currentVelocity = Vector3.MoveTowards(_currentVelocity, _targetVelocity, friction * timeStep);

        // Calculates the amount of force to apply based off the difference of the rigidbody's current velocity and
        // the desired target velocity - also works as a corrective force if the rigidbody's velocity is higher than
        // the given targetVelocity.
        _targetAcceleration = (_currentVelocity - _rigidbody.linearVelocity) / timeStep;

        // This limits the amount of force that can be applied by the acceleration amount.
        // Since the formula above can act as a 'corrective force' it's also important to limit how much it can
        // correct by otherwise it will just instantly stop and feel unrealistic. 
        return Vector3.ClampMagnitude(_targetAcceleration, friction);
    }
}