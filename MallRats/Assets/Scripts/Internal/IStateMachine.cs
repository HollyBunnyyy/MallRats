public interface IStateMachine<IState>
{
    public void SetState( IState stateToSet );
    public IState CurrentState { get; }
}
