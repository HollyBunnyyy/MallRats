public interface IState<IStateMachine>
{
    public void OnStateEnter();
    public void OnStateExit();
}
