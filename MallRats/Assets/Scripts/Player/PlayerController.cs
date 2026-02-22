using UnityEngine;

[RequireComponent(typeof(PlayerStateMachine))]
public class PlayerController : PawnController
{
    [Header("Player Settings")]

    [SerializeField][Min(0.0f)]
    private float _movementSpeed = 5.0f;
    public float MovementSpeed 
    { 
        get { return _movementSpeed; } 
    }

    [SerializeField]
    private float _movementFriction = 24.0f;
    public float MovementFriction 
    { 
        get { return _movementFriction; } 
    }

    [SerializeField]
    private float _jumpForce = 1.0f;
    public float JumpForce 
    { 
        get { return _jumpForce; } 
    }

    public bool AllowInput = true;

    [Header("Components")]

    [SerializeField]
    private Camera _camera;
    public Camera Camera
    {
        get { return _camera; }
    }

    [SerializeField]
    private InputHandler _inputHandler;
    public InputHandler InputHandler
    {
        get { return _inputHandler; }
    }

    private PlayerStateMachine _stateMachine;
    public PlayerStateMachine StateMachine
    {
        get { return _stateMachine; }
    }

    protected void Start()
    {
        _stateMachine = GetComponent<PlayerStateMachine>();
        _stateMachine.SetState(new PlayerStates.PlayerWalkState(_stateMachine, this));
    }

    public Vector3 GetMovementDirection()
    {
        return Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0) * _inputHandler.MovementAxis;
    }

    public override void MoveTowards(Vector3 direction, float force, float friction)
    {
        base.MoveTowards(AllowInput is false ? Vector3.zero : direction, force, friction);
    }

    public void MoveTowards(Vector3 direction)
    {
        MoveTowards(direction, MovementSpeed, MovementFriction);
    }

    public void Jump(float force)
    {
        //magic numbers are for a formula I swear - sqrt(2 * height * gravity)
        Rigidbody.AddForce(Vector3.up * Mathf.Sqrt(2.0f * force * Gravity), ForceMode.Impulse);
    }
}
