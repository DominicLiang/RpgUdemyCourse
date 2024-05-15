public class SkeletonHitState : SkeletonState
{
    public SkeletonHitState(FSM fsm, Skeleton character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        if (Character.damageFrom)
        {
            var isRight = Character.damageFrom.transform.position.x > Character.transform.position.x;
            var isLeft = Character.damageFrom.transform.position.x < Character.transform.position.x;
            var moveDir = isRight ? 1 : isLeft ? -1 : 0;
            SetVelocity(moveDir * -1 * Character.knockbackXSpeed, Rb.velocity.y, false);
        }

        if (IsAnimationFinished)
            Fsm.SwitchState(Character.ChaseState);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
