public class JumpState : AirState
{
    public JumpState(FSM fsm, Player player, string animBoolName) : base(fsm, player, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        if (airJumpCounter >= Character.airJumpCount)
        {
            return;
        }

        if (!ColDetect.isGrounded)
        {
            airJumpCounter++;
        }

        SetVelocity(Rb.velocity.x, Character.jumpForce);
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
