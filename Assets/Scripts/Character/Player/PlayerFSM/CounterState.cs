using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterState : PlayerState
{
    public CounterState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        StateTimer = Character.counterDuration;
        Anim.SetBool("CounterSuccess", false);
    }

    public override void Update()
    {
        base.Update();

        SetVelocity(0, 0);

        Collider2D[] coliders = Physics2D.OverlapCircleAll(Character.attackCheck.position, Character.attackCheckRadius);

        foreach (var hit in coliders)
        {
            if (hit.transform == Character.transform) continue;
            var enemy = hit.GetComponent<Skeleton>();
            if (!enemy) continue;
            if (enemy.CanBeStun())
            {
                StateTimer = 10;
                Anim.SetBool("CounterSuccess", true);
                var damageable = hit.GetComponent<Damageable>();
                if (!damageable) continue;
                damageable.TakeDamage(Character.gameObject, damageable.gameObject, 1);
            }
        }

        if (StateTimer < 0 || IsAnimationFinished)
        {
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
