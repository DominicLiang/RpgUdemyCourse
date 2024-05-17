using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public float cooldown;
    protected Player player;
    private float cooldownTimer;

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
}
