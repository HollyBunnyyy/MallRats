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
    private CameraController _cameraController;

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

    private GridInventory<Item> _inventoryTest;

    [SerializeField]
    private GridGraphicController _inventoryUI;

    private bool _isInventoryOpen;

    protected void Start()
    {
        _stateMachine = GetComponent<PlayerStateMachine>();
        _stateMachine.SetState(new PlayerStates.PlayerWalkState(_stateMachine, this));

        _inventoryTest = new GridInventory<Item>(8, 5);

        _inventoryTest.AddItem(3, 4, new Item(3, 4));

        _inventoryUI.SetInventory(_inventoryTest);    
    }

    protected override void Update()
    {
        base.Update();

        if (_inputHandler.ShouldInventory)
        {
            _isInventoryOpen = !_isInventoryOpen;

            _inventoryUI.gameObject.SetActive(_isInventoryOpen);
            _cameraController.CanReceieveInput = !_isInventoryOpen;

            Cursor.visible = _isInventoryOpen;
            Cursor.lockState = _isInventoryOpen ? CursorLockMode.Confined : CursorLockMode.Locked;
        }

        if (_inputHandler.ShouldInteract)
        {
            _inventoryTest.AddItem(2, 2, new Item(2, 2));
        }
    }

    public Vector3 GetMovementDirection()
    {
        return Quaternion.Euler(0, _cameraController.transform.eulerAngles.y, 0) * _inputHandler.MovementAxis;
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
