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

    protected override void SwitchHitState()
    {
        Fsm.SwitchState(HitState);
    }

    protected override void SwitchStunState()
    {
        Fsm.SwitchState(StunState);
    }

    protected override bool IsInStunState()
    {
        return Fsm.CurrentState == StunState;
    }

    public override void Die()
    {
        Fsm.SwitchState(DeadState);
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    public override void SlowBy(float slowPercentage, float slowDuration)
    {
        StartCoroutine(Slow(slowPercentage, slowDuration));
        IEnumerator Slow(float slowPercentage, float slowDuration)
        {
            var slow = 1 - slowPercentage;
            Anim.speed = slow;
            moveSpeed *= slow;
            yield return new WaitForSeconds(slowDuration);
            Anim.speed = 1;
            moveSpeed = defaultMoveSpeed;
        }
    }
}
