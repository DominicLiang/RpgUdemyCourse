using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Value")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private Color cloneColor;
    [SerializeField] private bool canAttack;

    public void CreateClone(Vector3 position, Quaternion rotation)
    {
        var newClone = Instantiate(clonePrefab);
        newClone.transform.SetParent(PlayerManager.Instance.fx.transform);
        var controller = newClone.GetComponent<CloneSkillController>();
        controller.Setup(position, rotation, cloneDuration, cloneColor, canAttack);
    }

    protected override void SkillFunction()
    {

    }
}