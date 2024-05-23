using UnityEngine;

public class EnemyDamageable : Damageable
{
    private ItemDrop drop;

    protected override void Start()
    {
        base.Start();
        drop = GetComponent<ItemDrop>();
    }

    protected override void Die()
    {
        if (!TryGetComponent(out Enemy enemy)) return;
        drop.GenerateDrop();
        enemy.Die();
    }
}