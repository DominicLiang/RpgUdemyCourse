using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float duration;

    [Header("Crystal Mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Growing Crystal")]
    [SerializeField] private Vector2 maxSize;
    [SerializeField] private float growSpeed;


    [Header("Explosive Crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private bool canMove;
    [SerializeField] private float moveSpeed;

    [Header("Multi Stacking Crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft;

    private GameObject currentCrystal;

    protected override void SkillFunction()
    {
        if (CanUseMultiCrystal()) return;

        if (currentCrystal == null)
        {
            currentCrystal = CreateCrystal(crystalPrefab);
        }
        else
        {
            if (canMove) return;
            var playerPos = player.transform.position + new Vector3(0, 1);
            var crystalPos = currentCrystal.transform.position + new Vector3(0, -1);
            player.transform.position = crystalPos;
            currentCrystal.transform.position = playerPos;
            if (cloneInsteadOfCrystal)
            {
                var pos = currentCrystal.transform.position + new Vector3(0, -1);
                SkillManager.Instance.Clone.CreateClone(pos, player.transform.rotation, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                if (currentCrystal.TryGetComponent(out CrystalSkillController sc))
                {
                    sc.FinishCrystal();
                }
            }
        }
    }

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                {
                    Invoke(nameof(ResetAbility), useTimeWindow);
                }

                cooldown = 0;
                var crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                CreateCrystal(crystalToSpawn);
                crystalLeft.Remove(crystalToSpawn);

                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }
                return true;
            }
        }
        return false;
    }

    public void CreateCrystal()
    {
        currentCrystal = CreateCrystal(crystalPrefab);
    }

    private GameObject CreateCrystal(GameObject crystalToSpawn)
    {
        var pos = player.transform.position + new Vector3(0, 1);
        var parent = PlayerManager.Instance.fx.transform;
        var newCrystal = Instantiate(crystalToSpawn, pos, Quaternion.identity, parent);
        if (newCrystal.TryGetComponent(out CrystalSkillController sc))
        {
            sc.Setup(
                player,
                duration,
                canExplode,
                canMove,
                moveSpeed,
                maxSize,
                growSpeed,
                FindClosestEnemy);
        }
        return newCrystal;
    }

    public void CurrentCrystalChooseRandomTarget()
    {
        if (!currentCrystal.TryGetComponent(out CrystalSkillController sc)) return;
        sc.ChooseRandomEnemy();
    }

    private void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0) return;
        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }
}
