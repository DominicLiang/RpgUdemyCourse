using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;

    private float cloneDuration;
    private Color cloneColor;
    private float cloneTimer;
    private Transform closeEnemy;
    private Animator anim;
    private SpriteRenderer sr;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

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

    public void Setup(
        Vector3 position,
        Quaternion rotation,
        float cloneDuration,
        Color cloneColor,
        bool canAttack)
    {
        cloneTimer = cloneDuration;
        this.cloneDuration = cloneDuration;
        this.cloneColor = cloneColor;

        if (canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 3));

        PositionRotationSetup(position, rotation);
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

        var collider = Physics2D.OverlapCircleAll(transform.position, 3);

        var closeDis = Mathf.Infinity;

        foreach (var hit in collider)
        {
            if (!hit.CompareTag("Enemy")) continue;
            var disToEnemy = Vector2.Distance(transform.position, hit.transform.position);
            if (disToEnemy >= closeDis) continue;
            closeDis = disToEnemy;
            closeEnemy = hit.transform;
        }

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
