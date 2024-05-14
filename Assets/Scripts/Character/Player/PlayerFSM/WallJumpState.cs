public class WallJumpState : AirState
{
    public WallJumpState(FSM fsm, Player player, string animBoolName) : base(fsm, player, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        SetVelocity(Character.wallJumpXSpeed * Flip.facingDir * -1, Character.jumpForce);

        airJumpCounter++;
    }

    public override void Update()
    {
        base.Update();

        if (Rb.velocity.y < 0)
        {
            Fsm.SwitchState(Character.FallState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
