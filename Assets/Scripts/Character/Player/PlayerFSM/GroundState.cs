public class GroundState : PlayerState
{
    public GroundState(FSM fsm, Player player, string animBoolName) : base(fsm, player, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        if (Input.isAttackPressed)
        {
            Fsm.SwitchState(Character.AttackState);
        }

        if (Input.isJumpDown && ColDetect.isGrounded)
        {
            Fsm.SwitchState(Character.JumpState);
        }

        if (!ColDetect.isGrounded)
        {
            Fsm.SwitchState(Character.FallState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
