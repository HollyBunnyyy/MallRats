using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputHandler")]
public class InputHandler : ScriptableObject, InputMap.IInputActions
{
    [SerializeField]
    private InputMap _inputMap;

    [SerializeField]
    private float _mouseScaling = 0.05f;

    public Mouse CurrentMouse 
    { 
        get { return Mouse.current; } 
    }

    public Vector3 MousePosition
    {
        get { return CurrentMouse.position.ReadValue(); }
    }

    public Vector3 MousePositionDelta
    {
        get { return CurrentMouse.delta.ReadValue() * _mouseScaling; }
    }

    public bool LeftMouseDown
    {
        get { return CurrentMouse.leftButton.wasPressedThisFrame; }
    }

    public Vector3 MovementAxis  
    { 
        get { return _inputMap.Input.Move.ReadValue<Vector3>().normalized; } 
    }

    public bool ShouldInteract
    {
        get { return _inputMap.Input.Interact.WasPressedThisFrame(); }
    }

    public bool ShouldInteractContinuous
    {
        get { return _inputMap.Input.Interact.inProgress; }
    }

    public bool ShouldMove
    {
        get { return _inputMap.Input.Move.inProgress; }
    }

    public bool ShouldJump
    {
        get { return _inputMap.Input.Jump.WasPressedThisFrame(); }
    }

    public bool ShouldCrouch
    {
        get { return _inputMap.Input.Crouch.inProgress; }
    }

    public bool ShouldSprint
    {
        get { return _inputMap.Input.Sprint.inProgress; }
    }

    public bool ShouldInventory
    {
        get { return _inputMap.Input.Inventory.WasPressedThisFrame(); }
    }

    public event Action JumpEvent;
    public event Action CrouchEvent;
    public event Action SprintEvent;
    public event Action InteractEvent;
    public event Action<Vector3> MoveEvent;
    public event Action InventoryEvent;

    protected void OnEnable()
    {
        _inputMap ??= new InputMap();

        _inputMap.Input.SetCallbacks(this);
        _inputMap.Enable();
    }

    protected void OnDisable()
    {
        _inputMap.Input.SetCallbacks(null);
        _inputMap.Disable();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        CrouchEvent?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        JumpEvent?.Invoke();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        SprintEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector3>());
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        InteractEvent?.Invoke();
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        InventoryEvent?.Invoke();
    }
}