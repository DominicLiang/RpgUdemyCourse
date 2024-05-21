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
    private bool canDuplicateClone;
    private float duplicateProbability;
    private int facingDir;
    private Func<Transform, float, Transform> findClosestEnemy;
    private Transform closeEnemy;
    private Animator anim;
    private SpriteRenderer sr;
    private Player player;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        facingDir = 1;
    }

    public void Setup(
        Player player,
        Vector3 position,
        Quaternion rotation,
        float cloneDuration,
        Color cloneColor,
        bool isDelayAttack,
        Vector3 offset,
        float attackDelay,
        bool canDuplicateClone,
        float duplicateProbability,
        Func<Transform, float, Transform> findClosestEnemy)
    {
        this.player = player;
        cloneTimer = cloneDuration;
        this.cloneDuration = cloneDuration;
        this.cloneColor = cloneColor;
        this.canDuplicateClone = canDuplicateClone;
        this.findClosestEnemy = findClosestEnemy;
        this.duplicateProbability = duplicateProbability;

        if (isDelayAttack)
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
            if (hit.CompareTag("Player")
                || hit.transform == transform.parent
                || !hit.TryGetComponent(out Damageable damageable)) continue;
            damageable.TakeDamage(player.gameObject);

            if (!canDuplicateClone || UnityEngine.Random.Range(0, 100) >= duplicateProbability) continue;
            SkillManager.Instance.Clone.CreateClone(hit.transform.position, player.transform.rotation, new Vector3(1.5f * facingDir, 0));
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
            facingDir = -1;
        }
    }
}
