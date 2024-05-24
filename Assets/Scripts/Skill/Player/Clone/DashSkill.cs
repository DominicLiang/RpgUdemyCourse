using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    public bool dashUnlocked;
    public SkillTreeSlot dashUnlockedButton;

    [Header("CloneOnDash")]
    public bool cloneOnDashUnlocked;
    public SkillTreeSlot cloneOnDashUnlockedButton;

    [Header("CloneOnArrival")]
    public bool cloneOnArrivalUnlocked;
    public SkillTreeSlot cloneOnArrivalUnlockedButton;

    protected override void SkillFunction()
    {
    }

    protected override void Start()
    {
        base.Start();

        dashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
    }

    private void UnlockDash()
    {
        if (dashUnlockedButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockCloneOnDash()
    {
        cloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival()
    {
        cloneOnArrivalUnlocked = true;
    }
}
