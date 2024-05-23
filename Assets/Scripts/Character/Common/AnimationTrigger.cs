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
        var colliders = Physics2D.OverlapCircleAll(character.attackCheck.position, character.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.transform == transform.parent) continue;
            if (transform.parent.CompareTag("Enemy") && hit.CompareTag("Enemy")) continue;
            hit.GetComponent<Damageable>()?.TakeDamage(transform.parent.gameObject);
        }
    }
}
