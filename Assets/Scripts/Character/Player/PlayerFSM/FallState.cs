public class FallState : AirState
{
    public FallState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        if (ColDetect.IsWallDetected)
        {
            Fsm.SwitchState(Character.WallSlideState);
        }

        if (ColDetect.IsGrounded)
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
