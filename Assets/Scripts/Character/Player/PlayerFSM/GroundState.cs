using UnityEngine;

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

        if (UnityEngine.Input.GetKeyDown(KeyCode.F))
        {
            Fsm.SwitchState(Character.BlackholeState);
            return;
        }

        if (!ColDetect.IsGrounded)
        {
            Fsm.SwitchState(Character.FallState);
            return;
        }

        if (Input.isJumpDown)
        {
            Fsm.SwitchState(Character.JumpState);
            return;
        }

        if (Input.isAttackPressed)
        {
            Fsm.SwitchState(Character.AttackState);
            return;
        }

        if (Input.isCounterDown)
        {
            Fsm.SwitchState(Character.CounterState);
            return;
        }

        if (Input.isAimSwordDown && CanReturnSword())
        {
            Fsm.SwitchState(Character.AimSwordState);
            return;
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    private bool CanReturnSword()
    {
        if (!Character.UsedSword) return true;
        Character.UsedSword.GetComponent<SwordSKillController>().ReturnSword();
        return false;
    }
}
