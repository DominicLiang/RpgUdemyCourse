using UnityEngine;

public class PlayerDamageable : Damageable
{
    protected override void Die()
    {
        if (!TryGetComponent(out Player player)) return;
        player.Die();
    }
}