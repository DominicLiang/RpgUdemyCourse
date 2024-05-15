public interface IState
{
    public void Enter(IState lastState);
    public void Update();
    public void Exit(IState newState);
    public void AnimationFinishTrigger();
}
