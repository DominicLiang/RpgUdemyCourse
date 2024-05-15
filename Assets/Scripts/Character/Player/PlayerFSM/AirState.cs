public class AirState : PlayerState
{
    protected static int airJumpCounter;

    public AirState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        if (Input.isJumpDown)
        {
            Fsm.SwitchState(Character.JumpState);
        }

        if (Input.xAxis != 0)
        {
            SetVelocity(Character.moveSpeed * Input.xAxis, Rb.velocity.y);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
