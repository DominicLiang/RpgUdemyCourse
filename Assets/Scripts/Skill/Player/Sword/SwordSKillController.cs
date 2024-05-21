using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordSKillController : MonoBehaviour
{
    private SwordType swordType;

    private bool isBouncing;
    private float bounceSpeed;
    private int bounceAmount;
    private List<Transform> enemyTargets;
    private int targetIndex;

    private bool isPierce;
    private float pierceAmount;

    private bool isSpinning;
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private float hitTimer;
    private float hitCooldown;

    private float returnSpeed;
    private bool canRotate;
    private bool isReturning;

    private Vector2 pos;
    private Vector2 playerPos;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();

        canRotate = true;
        enemyTargets = new List<Transform>();
    }

    public void Setup(
        SwordType swordType,
        Player player,
        Vector2 aimDir,
        float gravityScale,
        float returnSpeed,
        float bounceSpeed,
        int bounceAmount,
        int pierceAmount,
        float maxTravelDistance,
        float spinDuration,
        float hitCooldown)
    {
        this.swordType = swordType;
        this.player = player;
        rb.velocity = aimDir;
        rb.gravityScale = gravityScale;
        this.returnSpeed = returnSpeed;
        this.bounceSpeed = bounceSpeed;
        this.bounceAmount = bounceAmount;
        this.pierceAmount = pierceAmount;
        this.maxTravelDistance = maxTravelDistance;
        this.spinDuration = spinDuration;
        this.hitCooldown = hitCooldown;

        if (swordType == SwordType.Bounce)
            isBouncing = true;

        if (swordType == SwordType.Spin)
            isSpinning = true;

        if (swordType == SwordType.Pierce)
        {
            isPierce = true;
        }
        else
        {
            anim.SetBool("Rotation", true);
        }

        Invoke(nameof(DestoryMe), 5);
    }

    private void DestoryMe()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        pos = transform.position;
        playerPos = player.transform.position + new Vector3(0, 1, 0);

        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        ReturnLogic();

        switch (swordType)
        {
            case SwordType.Bounce:
                BounceLogic();
                break;
            case SwordType.Spin:
                SpinLogic();
                break;
            default:
                break;
        }
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.SetParent(PlayerManager.Instance.fx.transform);
        isReturning = true;
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void ReturnLogic()
    {
        if (!isReturning) return;
        cd.enabled = true;
        anim.SetBool("Rotation", true);
        transform.position = Vector2.MoveTowards(pos, playerPos, returnSpeed * Time.deltaTime);
        if (Vector2.Distance(pos, playerPos) >= 1) return;
        player.CatchSword();
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            var enemyPos = enemyTargets[targetIndex].position + new Vector3(0, 1, 0);
            transform.position = Vector2.MoveTowards(pos, enemyPos, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(pos, enemyPos) < 0.1f)
            {
                TakeDamage(enemyTargets[targetIndex].GetComponent<Collider2D>(), true);
                targetIndex = (targetIndex + 1) % enemyTargets.Count;
                bounceAmount--;
                if (bounceAmount <= 0)
                {
                    enemyTargets.Clear();
                    isReturning = true;
                    isBouncing = false;
                }
            }
        }
    }

    private void SpinLogic()
    {
        if (Vector2.Distance(playerPos, pos) > maxTravelDistance && !wasStopped)
        {
            wasStopped = true;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            spinTimer = spinDuration;
        }

        if (!wasStopped) return;

        spinTimer -= Time.deltaTime;
        if (spinTimer < 0)
        {
            isReturning = true;
            isSpinning = false;
        }

        hitTimer -= Time.deltaTime;
        if (hitTimer >= 0) return;

        hitTimer = hitCooldown;

        var colliders = Physics2D.OverlapCircleAll(transform.position, 1);
        foreach (var hit in colliders)
        {
            TakeDamage(hit, false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Platform"))
        {
            if (other.CompareTag("Player") || !other.CompareTag("Enemy")) return;

            if (!isSpinning && !isBouncing) TakeDamage(other, true);

            GetBounceEnemy();

            if (CancelStuck()) return;
        }

        StuckInTo(other);
    }

    private void TakeDamage(Collider2D other, bool needFreeze)
    {
        if (!other.TryGetComponent(out Damageable damageable)) return;
        damageable.TakeDamage(player.gameObject);
        if (!needFreeze) return;
        if (!other.TryGetComponent(out Enemy enemy)) return;
        enemy.FreezeTimeForSeconds(0.7f);
    }

    private void GetBounceEnemy()
    {
        if (!isBouncing || enemyTargets.Count > 0) return;
        var colliders = Physics2D.OverlapCircleAll(transform.position, 10);
        foreach (var hit in colliders)
        {
            if (!hit.CompareTag("Enemy")) continue;
            enemyTargets.Add(hit.transform);
        }
    }

    private bool CancelStuck()
    {
        if (isSpinning)
        {
            wasStopped = true;
            cd.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            spinTimer = spinDuration;
            return true;
        }

        if (isPierce && pierceAmount > 0)
        {
            pierceAmount--;
            return true;
        }

        if (isBouncing && enemyTargets.Count > 0)
        {
            canRotate = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            return true;
        }

        return false;
    }

    private void StuckInTo(Collider2D other)
    {
        canRotate = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        cd.enabled = false;
        transform.SetParent(other.transform);
        anim.SetBool("Rotation", false);
    }
}
