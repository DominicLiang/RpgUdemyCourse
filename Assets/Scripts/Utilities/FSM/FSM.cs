public class FSM
{
    public IState currentState { get; private set; }

    public void SwitchState(IState newState)
    {
        var lastState = currentState;
        lastState?.Exit(newState);
        currentState = newState;
        newState.Enter(lastState);
    }
}
