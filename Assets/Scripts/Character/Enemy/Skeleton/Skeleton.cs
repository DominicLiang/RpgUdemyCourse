using UnityEngine;

[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(FlashFX))]
public class Skeleton : Character
{
    #region Value
    [Header("Attack Value")]
    public float attackDistance = 2f;
    public float attackCooldown = 0.4f;
    public float lostPlayerTime = 7f;

    [Header("Stun Value")]
    public float stunTime = 2f;
    public bool canBeStun = true;
    public GameObject counterImage;
    #endregion

    #region Component
    public Damageable Damageable { get; private set; }
    public FlashFX FlashFX { get; private set; }
    #endregion

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

        Damageable = GetComponent<Damageable>();
        FlashFX = GetComponent<FlashFX>();

        Damageable.onTakeDamage += (from, to) =>
        {
            damageFrom = from;
            Fsm.SwitchState(HitState);
        };

        counterImage = transform.Find("CounterImage").gameObject;

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

    public virtual void OpenCounterAttackWindow()
    {
        canBeStun = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStun = false;
        counterImage.SetActive(false);
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
