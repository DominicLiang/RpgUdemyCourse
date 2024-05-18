using System.Collections;
using UnityEngine;

public class Skeleton : Enemy
{
    #region State
    public IState IdleState { get; private set; }
    public IState PatrolState { get; private set; }
    public IState ChaseState { get; private set; }
    public IState AttackState { get; private set; }
    public IState HitState { get; private set; }
    public IState DeadState { get; private set; }
    public IState StunState { get; private set; }
    #endregion

    protected override void Start()
    {
        base.Start();

        Damageable.onTakeDamage += (from, to) =>
        {
            damageFrom = from;
            if (Fsm.CurrentState == StunState)
            {
                if (damageFrom)
                {
                    var isRight = damageFrom.transform.position.x > transform.position.x;
                    var isLeft = damageFrom.transform.position.x < transform.position.x;
                    var faceDir = isRight ? 1 : isLeft ? -1 : 0;
                    Rb.velocity = new Vector2(faceDir * -1 * knockbackXSpeed, knockbackYSpeed);
                    if (faceDir != 0 && faceDir != Flip.facingDir) Flip.Flip();
                }
                return;
            }
            Fsm.SwitchState(HitState);
        };

        IdleState = new SkeletonIdleState(Fsm, this, "Idle");
        PatrolState = new SkeletonPatrolState(Fsm, this, "Move");
        ChaseState = new SkeletonChaseState(Fsm, this, "Move");
        AttackState = new SkeletonAttackState(Fsm, this, "Attack");
        HitState = new SkeletonHitState(Fsm, this, "Hit");
        DeadState = new SkeletonDeadState(Fsm, this, "Dead");
        StunState = new SkeletonStunState(Fsm, this, "Stun");
        Fsm.SwitchState(IdleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public virtual bool CanBeStun()
    {
        if (canBeStun)
        {
            Fsm.SwitchState(StunState);
            CloseCounterAttackWindow();
            return true;
        }
        else
        {
            return false;
        }
    }
}
