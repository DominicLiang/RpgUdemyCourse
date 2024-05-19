using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public float cooldown;
    protected Player player;
    protected float cooldownTimer;

    protected virtual void Start()
    {
        player = PlayerManager.Instance.player.GetComponent<Player>();
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public bool CanUseSkill()
    {
        if (cooldownTimer >= 0) return false;
        SkillFunction();
        cooldownTimer = cooldown;
        return true;
    }

    protected abstract void SkillFunction();

    protected virtual Transform FindClosestEnemy(Transform detectTransform, float radius)
    {
        var collider = Physics2D.OverlapCircleAll(detectTransform.position, radius);

        var closeDis = Mathf.Infinity;

        Transform closeEnemy = null;
        foreach (var hit in collider)
        {
            if (!hit.CompareTag("Enemy")) continue;
            var disToEnemy = Vector2.Distance(detectTransform.position, hit.transform.position);
            if (disToEnemy >= closeDis) continue;
            closeDis = disToEnemy;
            closeEnemy = hit.transform;
        }
        return closeEnemy;
    }
}
