public class FallState : AirState
{
    public FallState(FSM fsm, Player player, string animBoolName) : base(fsm, player, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        if (ColDetect.isWallDetected)
        {
            Fsm.SwitchState(Character.WallSlideState);
        }

        if (ColDetect.isGrounded)
        {
            Fsm.SwitchState(Character.IdleState);
            airJumpCounter = 0;
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
