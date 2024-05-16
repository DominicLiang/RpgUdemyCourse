public class GroundState : PlayerState
{
    public GroundState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        if (Input.isCounterDown && ColDetect.IsGrounded)
        {
            Fsm.SwitchState(Character.CounterState);
        }

        if (Input.isAttackPressed && ColDetect.IsGrounded)
        {
            Fsm.SwitchState(Character.AttackState);
        }

        if (Input.isJumpDown && ColDetect.IsGrounded)
        {
            Fsm.SwitchState(Character.JumpState);
        }

        if (!ColDetect.IsGrounded)
        {
            Fsm.SwitchState(Character.FallState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
