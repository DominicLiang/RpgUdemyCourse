using UnityEngine;

public class AnimationTrigger<T> : MonoBehaviour where T : Character
{
    protected T character;

    private void Start()
    {
        character = GetComponentInParent<T>();
        if (!character) Debug.LogError($"父物体未找到名为 {typeof(T)} 的组件");
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
            if (hit.transform == transform.parent) continue;
            var damageable = hit.GetComponent<Damageable>();
            if (!damageable) continue;
            damageable.TakeDamage(gameObject, damageable.gameObject, 1);
        }
    }
}
