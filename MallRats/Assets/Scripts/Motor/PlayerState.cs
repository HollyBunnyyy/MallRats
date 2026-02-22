public class PlayerState : IState<PlayerStateMachine>
{
    protected readonly PlayerStateMachine StateMachineContext;
    protected readonly PlayerController PlayerContext;

    public PlayerState(PlayerStateMachine stateMachineContext, PlayerController playerController)
    {
        StateMachineContext = stateMachineContext;
        PlayerContext       = playerController;
    }

    /// <summary>
    /// Called once when entering the state.
    /// </summary>
    public virtual void OnStateEnter() { }

    /// <summary>
    /// Called every update frame.
    /// </summary>
    public virtual void OnStateUpdate() { }

    /// <summary>
    /// Called every FixedUpdate frame.
    /// </summary>
    public virtual void OnStateFixedUpdate() { }

    /// <summary>
    /// Called every LateUpdate frame.
    /// </summary>
    public virtual void OnStateLateUpdate() { }

    /// <summary>
    /// Called once when exiting the state.
    /// </summary>
    public virtual void OnStateExit() { }
}
