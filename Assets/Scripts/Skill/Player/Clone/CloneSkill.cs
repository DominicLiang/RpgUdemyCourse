using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Value")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private Color cloneColor;
    [SerializeField] private bool canAttack;
    [SerializeField] private float attackDelay;

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool createCloneOnCounterAttack;

    public void CreateClone(Vector3 position, Quaternion rotation, Vector3 offset)
    {
        var newClone = Instantiate(clonePrefab);
        newClone.transform.SetParent(PlayerManager.Instance.fx.transform);
        var controller = newClone.GetComponent<CloneSkillController>();
        controller.Setup(
            position,
            rotation,
            cloneDuration,
            cloneColor,
            canAttack,
            offset,
            attackDelay,
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
