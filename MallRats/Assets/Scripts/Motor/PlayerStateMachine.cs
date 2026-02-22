using UnityEngine;

public class PlayerStateMachine : MonoBehaviour, IStateMachine<PlayerState>
{
    private PlayerState _currentState;
    public PlayerState CurrentState 
    {
        get { return _currentState; }
    }

    private PlayerState _previousState;
    public PlayerState PreviousState
    {
        get { return _previousState; }
    }

    protected void OnEnable()
    {
        _currentState?.OnStateEnter();
    }

    protected void Update()
    {
        _currentState?.OnStateUpdate();
    }

    protected void FixedUpdate()
    {
        _currentState?.OnStateFixedUpdate();
    }

    protected void LateUpdate()
    {
        _currentState?.OnStateLateUpdate();
    }

    protected void OnDisable()
    {
        _currentState?.OnStateExit();
    }

    public void SetState(PlayerState stateToSet)
    {
        _currentState?.OnStateExit();

        _previousState = _currentState;
        _currentState = stateToSet;

        _currentState?.OnStateEnter();
    }
}
