using System;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private float crystalExistTimer;

    private bool canExplode;

    private bool canMove;
    private float moveSpeed;

    private Vector2 maxSize;
    private bool canGrow;
    private float growSpeed;

    private Func<Transform, float, Transform> findClosestEnemy;
    private Transform randomTarget;

    private Player player;
    private Animator anim;
    private CircleCollider2D cd;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cd = GetComponent<CircleCollider2D>();
    }

    public void Setup(
        Player player,
        float duration,
        bool canExplode,
        bool canMove,
        float moveSpeed,
        Vector2 maxSize,
        float growSpeed,
        Func<Transform, float, Transform> findClosestEnemy)
    {
        this.player = player;
        crystalExistTimer = duration;
        this.canExplode = canExplode;
        this.canMove = canMove;
        this.moveSpeed = moveSpeed;
        this.maxSize = maxSize;
        this.growSpeed = growSpeed;
        this.findClosestEnemy = findClosestEnemy;
    }

    public void ChooseRandomEnemy()
    {
        var radius = SkillManager.Instance.Blackhole.GetBlackholeRadius();
        var colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        var colliderList = new List<Collider2D>();
        foreach (var collider in colliders)
        {
            if (!collider.CompareTag("Enemy")) continue;
            colliderList.Add(collider);
        }
        if (colliderList.Count <= 0) return;
        randomTarget = colliderList[UnityEngine.Random.Range(0, colliderList.Count)].transform;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (canMove)
        {
            var closestEnemy = randomTarget ? randomTarget : findClosestEnemy?.Invoke(transform, 10f);
            if (closestEnemy)
            {
                var enemyPos = closestEnemy.position + new Vector3(0, 1);
                transform.position = Vector2.MoveTowards(transform.position, enemyPos, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, enemyPos) < 0.1f)
                {
                    FinishCrystal();
                    canMove = false;
                }
            }
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, maxSize, growSpeed * Time.deltaTime);
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    private void AnimationExplodeEvent()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.CompareTag("Player") || hit.transform == transform) continue;
            if (!hit.TryGetComponent(out Damageable damageable)) continue;
            var equipedAmulet = Inventory.Instance.GetEquipmentByType(EquipmentType.Amulet);
            if (equipedAmulet != null)
                equipedAmulet.ExecuteItemEffect(player.gameObject, hit.gameObject);
            damageable.TakeDamage(player.gameObject, true);
        }
    }

    private void AnimationFinishEvent()
    {
        SelfDestroy();
    }
}
