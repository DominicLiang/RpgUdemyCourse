using System.Collections.Generic;
using UnityEngine;

public class SwordSKillController : MonoBehaviour
{
    private bool isBouncing;
    private float bounceSpeed;
    private int bounceAmount;
    private List<Transform> enemyTargets;
    private int targetIndex;

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

    private void DestoryMe()
    {
        Destroy(gameObject);
    }

    public void Setup(Vector2 dir, float gravityScale, float returnSpeed, Player player)
    {
        rb.velocity = dir;
        rb.gravityScale = gravityScale;
        this.returnSpeed = returnSpeed;
        this.player = player;

        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);

        Invoke(nameof(DestoryMe), 7);
    }

    public void SetupBounce(bool isBouncing, float bounceSpeed, int bounceAmount)
    {
        this.isBouncing = isBouncing;
        this.bounceSpeed = bounceSpeed;
        this.bounceAmount = bounceAmount;
    }

    public void SetupPierce(int pierceAmount)
    {
        this.pierceAmount = pierceAmount;
    }

    public void SetupSpin(bool isSpinning, float maxTravelDistance, float spinDuration, float hitCooldown)
    {
        this.isSpinning = isSpinning;
        this.maxTravelDistance = maxTravelDistance;
        this.spinDuration = spinDuration;
        this.hitCooldown = hitCooldown;
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        ReturnLogic();
        BounceLogic();

        if (isSpinning)
        {
            var playerPos = (Vector2)player.transform.position + new Vector2(0, 1);
            if (Vector2.Distance(playerPos, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
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
                if (!hit.CompareTag("Enemy")) continue;
                hit.GetComponent<Damageable>()?.TakeDamage(player.gameObject, hit.gameObject, 1);
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void ReturnLogic()
    {
        if (isReturning)
        {
            anim.SetBool("Rotation", true);
            var returnPos = player.transform.position + new Vector3(0, 1, 0);
            transform.position = Vector2.MoveTowards(transform.position, returnPos, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, returnPos) < 1)
            {
                player.CatchSword();
            }
        }
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            var enemyPos = (Vector2)enemyTargets[targetIndex].position + new Vector2(0, 1);
            transform.position = Vector2.MoveTowards(transform.position, enemyPos, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < 0.1f)
            {
                targetIndex = (targetIndex + 1) % enemyTargets.Count;
                bounceAmount--;
                if (bounceAmount <= 0)
                {
                    enemyTargets.Clear();
                    isReturning = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning || other.CompareTag("Player")) return;

        if (other.CompareTag("Enemy"))
        {
            var damageable = other.GetComponent<Damageable>();
            if (damageable)
                damageable.TakeDamage(player.gameObject, other.gameObject, 1);
            var enemy = other.GetComponent<Skeleton>();
            enemy.FreezeTimeForSeconds(0.7f);
        }

        if (other.CompareTag("Enemy"))
        {
            if (isBouncing && enemyTargets.Count <= 0)
            {
                var colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in colliders)
                {
                    if (!hit.CompareTag("Enemy")) continue;
                    enemyTargets.Add(hit.transform);
                }
            }
        }

        StuckInTo(other);
    }

    private void StuckInTo(Collider2D other)
    {
        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        if (pierceAmount > 0 && other.CompareTag("Enemy"))
        {
            pierceAmount--;
            return;
        }

        canRotate = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTargets.Count > 0) return;

        cd.enabled = false;
        transform.SetParent(other.transform);
        anim.SetBool("Rotation", false);
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.SetParent(PlayerManager.Instance.fx.transform);
        isReturning = true;
    }
}
