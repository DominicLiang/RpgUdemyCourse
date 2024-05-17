using UnityEngine;

public class CatchSwordState : PlayerState
{
    private Transform sword;

    public CatchSwordState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        sword = Character.UsedSword.transform;

        if (Character.transform.position.x > sword.position.x && Flip.facingDir == 1)
        {
            Flip.Flip();
        }
        else if (Character.transform.position.x < sword.position.x && Flip.facingDir == -1)
        {
            Flip.Flip();
        }

        SetVelocity(Character.swordReturnImpact * -Flip.facingDir, Rb.velocity.y, false);
    }

    public override void Update()
    {
        base.Update();

        if (IsAnimationFinished)
            Fsm.SwitchState(Character.IdleState);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
