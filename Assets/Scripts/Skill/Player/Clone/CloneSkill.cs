using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Value")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private Color cloneColor;
    [SerializeField] private bool isDelayAttack;
    [SerializeField] private float attackDelay;

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool createCloneOnCounterAttack;

    [Header("Duplicate Clone")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float duplicateProbability;

    [Header("Crystal Instead Of Clone")]
    [SerializeField] public bool crystalInsteadOfClone;

    public void CreateClone(Vector3 position, Quaternion rotation, Vector3 offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.Instance.Crystal.CreateCrystal();
            return;
        }

        var newClone = Instantiate(clonePrefab);
        newClone.transform.SetParent(PlayerManager.Instance.fx.transform);
        var controller = newClone.GetComponent<CloneSkillController>();
        controller.Setup(
            player,
            position,
            rotation,
            cloneDuration,
            cloneColor,
            isDelayAttack,
            offset,
            attackDelay,
            canDuplicateClone,
            duplicateProbability,
            FindClosestEnemy);
    }

    public void CreateCloneOnDashStart(Transform playerTransform)
    {
        if (!createCloneOnDashStart) return;
        CreateClone(playerTransform.position, playerTransform.rotation, Vector3.zero);
    }

    public void CreateCloneOnDashOver(Transform playerTransform)
    {
        if (!createCloneOnDashOver) return;
        CreateClone(playerTransform.position, playerTransform.rotation, Vector3.zero);
    }

    public void CreateCloneOnCounterAttack(Transform enemyTransform)
    {
        if (!createCloneOnCounterAttack) return;
        CreateClone(enemyTransform.position, enemyTransform.rotation, new Vector3(2 * player.Flip.facingDir, 0));
    }

    protected override void SkillFunction()
    {

    }
}
