using UnityEngine;

public class AnimationTrigger<T> : MonoBehaviour where T : Character
{
    private T character;

    private void Start()
    {
        character = GetComponentInParent<T>();
    }

    private void AnimationFinishTrigger()
    {
        character.Fsm.CurrentState?.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] coliders = Physics2D.OverlapCircleAll(character.attackCheck.position, character.attackCheckRadius);

        foreach (var hit in coliders)
        {
            var damageable = hit.GetComponent<Damageable>();
            if (!damageable) continue;
            damageable.TakeDamage(damageable.gameObject, 1);
        }
    }
}
