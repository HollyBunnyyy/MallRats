using UnityEngine;

[RequireComponent(typeof(Motor))]
public class PawnController : Pawn
{
    private Motor _motor;

    [Header("Pawn Settings")]

    [SerializeField][Min(0.01f)]
    private float _height = 0.5f;
    public float Height 
    { 
        get { return _height; }
    }

    [SerializeField]
    [Min(0.05f)]
    private float _stepHeight = 0.4f;
    public float StepHeight
    {
        get { return _stepHeight; }
    }

    [SerializeField]
    private float _gravity = 24.0f;
    public float Gravity
    {
        get { return _gravity; }
    }

    [SerializeField]
    private float _maxFallingSpeed = 40.0f;
    public float MaxFallingSpeed 
    {
        get { return _maxFallingSpeed; }
    }

    /// <summary>
    /// Specifies whether to apply a downwards gravity or not.
    /// </summary>
    public bool UseGravity = true;

    /// <summary>
    /// Specifies whether the pawn should actively snap it's position to Height amount off the ground.
    /// </summary>
    public bool SnapToGround = true;

    /// <summary>
    /// Returns whether the Pawn is actively on the ground or not.
    /// </summary>
    public bool IsGrounded
    {
        get { return _groundInfo.collider && (_groundInfo.distance <= Height) && Gravity > 0.0f; }
    }

    /// <summary>
    /// The hit info of the downwards-raycast.
    /// </summary>
    private RaycastHit _groundInfo;
    /// <inheritdoc cref="_groundInfo"/>
    public RaycastHit GroundInfo
    {
        get { return _groundInfo; }
    }

    protected override void Awake()
    {
        base.Awake();

        _motor = GetComponent<Motor>();
    }

    protected virtual void Update()
    {
        Physics.Raycast(transform.position, Vector3.down, out _groundInfo, 100.0f);

        if (SnapToGround && IsGrounded)
        {
            //Set the position of the transform "height" amount from the ground.
            Teleport(GroundInfo.point);
        }
    }
    
    protected virtual void FixedUpdate()
    {
        if (UseGravity && !IsGrounded)
        {
            Rigidbody.AddForce(Vector3.down * Gravity, ForceMode.Acceleration);
            Rigidbody.linearVelocity = Vector3.ClampMagnitude(Rigidbody.linearVelocity, MaxFallingSpeed);
        }
    }

    /// <summary>
    /// Moves towards the given direction by the specified force, multipled against the given friction.
    /// </summary>
    public virtual void Move(Vector3 direction, float force, float friction)
    {
        _motor.Move(direction.normalized, force, friction);
    }

    /// <summary>
    /// Moves towards the given direction by the specified force, multipled against the given friction.
    /// </summary>
    /// <remarks>* Additionally projects the movement vector onto the normal of the ground, use Move if this isn't desired.</remarks>
    public virtual void MoveTowards(Vector3 direction, float force, float friction)
    {
        _motor.Move(VectorUtility.ProjectOnPlane(direction, _groundInfo.normal).normalized, force, friction);
    }

    public void Teleport(Vector3 position)
    {
        transform.position = position + Vector3.up * Height;
    }
}
