using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(FlashFX))]
public class Skeleton : Character
{
    #region Value
    [Header("Move Value")]
    public float defaultMoveSpeed = 7f;
    [HideInInspector] public float moveSpeed;

    [Header("Attack Value")]
    public float attackDistance = 2f;
    public float attackCooldown = 0.4f;
    public float lostPlayerTime = 7f;

    [Header("Stun Value")]
    public float stunTime = 2f;
    public bool canBeStun = true;
    [HideInInspector] public GameObject counterImage;
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

        moveSpeed = defaultMoveSpeed;

        Damageable = GetComponent<Damageable>();
        FlashFX = GetComponent<FlashFX>();

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

    public void FreezeTimeForSeconds(float seconds)
    {
        StartCoroutine(FreezeTimeFor(seconds));
    }

    protected virtual IEnumerator FreezeTimeFor(float seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(seconds);
        FreezeTime(true);
    }

    public virtual void FreezeTime(bool toggle)
    {
        if (toggle)
        {
            moveSpeed = 0;
            Anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            Anim.speed = 1;
        }
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
