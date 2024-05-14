public class IdleState : GroundState
{
    public IdleState(FSM fsm, Player player, string animBoolName) : base(fsm, player, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();

        if (Input.xAxis != 0 && !isBusy)
        {
            Fsm.SwitchState(Character.MoveState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
