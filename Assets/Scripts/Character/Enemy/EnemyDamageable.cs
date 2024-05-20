using UnityEngine;

public class EnemyDamageable : Damageable
{
    protected override void Die()
    {
        if (!TryGetComponent(out Enemy enemy)) return;
        enemy.Die();
    }
}