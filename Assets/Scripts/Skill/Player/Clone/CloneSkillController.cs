using System;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;

    private float attackDelayTimer;
    private float cloneDuration;
    private Color cloneColor;
    private float cloneTimer;
    private Func<Transform, float, Transform> findClosestEnemy;
    private Transform closeEnemy;
    private Animator anim;
    private SpriteRenderer sr;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void Setup(
        Vector3 position,
        Quaternion rotation,
        float cloneDuration,
        Color cloneColor,
        bool canAttack,
        Vector3 offset,
        float attackDelay,
        Func<Transform, float, Transform> findClosestEnemy)
    {
        cloneTimer = cloneDuration;
        this.cloneDuration = cloneDuration;
        this.cloneColor = cloneColor;
        this.findClosestEnemy = findClosestEnemy;

        if (canAttack)// todo 延迟攻击
            attackDelayTimer = attackDelay;


        var pos = position + offset;

        PositionRotationSetup(pos, rotation);
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        attackDelayTimer -= Time.deltaTime;

        if (attackDelayTimer < 0)
        {
            attackDelayTimer = Mathf.Infinity;
            anim.SetInteger("AttackNumber", UnityEngine.Random.Range(1, 3));
        }

        if (cloneTimer < 0)
        {
            Destroy(gameObject);
        }
        else
        {
            cloneColor.a = cloneTimer / cloneDuration;
            sr.color = cloneColor;
        }
    }

    public void AnimationFinishTrigger()
    {
        anim.SetInteger("AttackNumber", 0);
    }

    public void AttackTrigger()
    {
        var colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.CompareTag("Player") || hit.transform == transform.parent) continue;
            var damageable = hit.GetComponent<Damageable>();
            if (!damageable) continue;
            damageable.TakeDamage(gameObject, damageable.gameObject, 1);
        }
    }

    private void PositionRotationSetup(Vector3 position, Quaternion rotation)
    {
        transform.position = position;

        closeEnemy = findClosestEnemy.Invoke(transform, 3f);

        if (!closeEnemy)
        {
            transform.rotation = rotation;
            return;
        }

        if (transform.position.x > closeEnemy.position.x)
        {
            transform.Rotate(0, 180, 0);
        }
    }
}
