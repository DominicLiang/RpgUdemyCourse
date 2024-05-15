public class FSM
{
    public IState CurrentState { get; private set; }

    public void SwitchState(IState newState)
    {
        var lastState = CurrentState;
        lastState?.Exit(newState);
        CurrentState = newState;
        newState.Enter(lastState);
    }
}
