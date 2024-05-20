using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkill : Skill
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackholeDuration;

    private BlackholeSkillController currentBlackhole;

    protected override void SkillFunction()
    {
        var pos = player.transform.position + new Vector3(0, 1);
        var parent = PlayerManager.Instance.fx.transform;
        var newBlackhole = Instantiate(blackholePrefab, pos, Quaternion.identity, parent);
        if (!newBlackhole.TryGetComponent(out currentBlackhole)) return;
        currentBlackhole.Setup(
            player,
            maxSize,
            growSpeed,
            shrinkSpeed,
            amountOfAttacks,
            cloneAttackCooldown,
            blackholeDuration);
    }

    public bool BlackholeFinished()
    {
        if (!currentBlackhole) return false;
        return currentBlackhole.PlayerCanExitState;
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}
