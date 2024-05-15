public class SkeletonIdleState : SkeletonGroundState
{
    public SkeletonIdleState(FSM fsm, Skeleton character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        SetVelocity(0, 0);

        StateTimer = 1f;
    }

    public override void Update()
    {
        base.Update();

        if (StateTimer < 0)
            Fsm.SwitchState(Character.PatrolState);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
