using UnityEngine;

public class AttackState : PlayerState
{
    private float lastAttackTime;
    private int comboCounter;

    public AttackState(FSM fsm, Player player, string animBoolName) : base(fsm, player, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        if (comboCounter >= Character.comboCount || Time.time >= lastAttackTime + Character.comboWindow)
        {
            comboCounter = 0;
        }

        Anim.SetInteger("ComboCounter", comboCounter);
        Anim.speed = Character.attackSpeed;

        var attackDir = Input.xAxis == 0 ? Flip.facingDir : Input.xAxis;
        SetVelocity(Character.attackMovement[comboCounter].x * attackDir, Character.attackMovement[comboCounter].y);

        StateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();

        if (StateTimer < 0)
        {
            SetVelocity(0, 0);
        }

        if (IsAnimationFinished)
        {
            Fsm.SwitchState(Character.IdleState);
        }

        if (!ColDetect.isGrounded)
        {
            Fsm.SwitchState(Character.FallState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);

        BusyFor(0.15f);

        Anim.speed = 1;

        comboCounter++;
        lastAttackTime = Time.time;
    }
}
