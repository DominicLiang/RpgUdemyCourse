public class WallSlideState : PlayerState
{
    public WallSlideState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        SetVelocity(0, Input.yAxis < 0 ? Rb.velocity.y : Rb.velocity.y * Character.slideSpeed);

        var x = Input.xAxis == 0 ? 0 : Input.xAxis > 0 ? 1 : -1;
        if (ColDetect.IsGrounded || (Input.xAxis != 0 && Flip.facingDir != x))
        {
            Fsm.SwitchState(Character.IdleState);
        }

        if (Input.isJumpDown)
        {
            Fsm.SwitchState(Character.WallJumpState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
