using UnityEngine;

public class CounterState : PlayerState
{
    private bool canCreateClone;

    public CounterState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        canCreateClone = true;

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
            if (!hit.CompareTag("Enemy")) continue;

            var enemy = hit.GetComponent<Enemy>();

            if (!enemy || !enemy.CanBeStun()) continue;

            StateTimer = 10;
            Anim.SetBool("CounterSuccess", true);

            if (canCreateClone)
            {
                SkillManager.Instance.Clone.CreateCloneOnCounterAttack(hit.transform);
                canCreateClone = false;
            }

            if (!hit.TryGetComponent(out Damageable to)) return;
            to.TakeDamage(Character.gameObject);
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
