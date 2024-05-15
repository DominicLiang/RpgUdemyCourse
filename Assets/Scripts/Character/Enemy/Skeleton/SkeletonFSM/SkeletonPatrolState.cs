public class SkeletonPatrolState : SkeletonGroundState
{
    public SkeletonPatrolState(FSM fsm, Skeleton character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        SetVelocity(Flip.facingDir * Character.moveSpeed, Rb.velocity.y);

        if (ColDetect.IsWallDetected || !ColDetect.IsGrounded)
        {
            Flip.Flip();
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
