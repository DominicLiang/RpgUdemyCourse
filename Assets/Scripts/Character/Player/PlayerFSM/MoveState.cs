public class MoveState : GroundState
{
    public MoveState(FSM fsm, Player player, string animBoolName) : base(fsm, player, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();

        SetVelocity(Input.xAxis * Character.moveSpeed, Rb.velocity.y);

        if (Input.xAxis == 0)
        {
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
