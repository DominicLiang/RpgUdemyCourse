using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Skeleton : Character
{
    #region Value
    [Header("Attack Value")]
    public float attackDistance = 2f;
    public float attackCooldown = 0.4f;
    public float lostPlayerTime = 7f;
    #endregion

    #region Component
    public Damageable damageable { get; private set; }
    #endregion

    #region State
    public IState IdleState { get; private set; }
    public IState PatrolState { get; private set; }
    public IState ChaseState { get; private set; }
    public IState AttackState { get; private set; }
    public IState HitState { get; private set; }
    public IState DeadState { get; private set; }
    #endregion

    protected override void Start()
    {
        base.Start();

        damageable = GetComponent<Damageable>();
        damageable.onTakeDamage += (from, to) =>
        {
            damageFrom = from;
            Fsm.SwitchState(HitState);
        };

        IdleState = new SkeletonIdleState(Fsm, this, "Idle");
        PatrolState = new SkeletonPatrolState(Fsm, this, "Move");
        ChaseState = new SkeletonChaseState(Fsm, this, "Move");
        AttackState = new SkeletonAttackState(Fsm, this, "Attack");
        HitState = new SkeletonHitState(Fsm, this, "Hit");
        DeadState = new SkeletonDeadState(Fsm, this, "Dead");
        Fsm.SwitchState(IdleState);
    }

    protected override void Update()
    {
        base.Update();
    }
}
