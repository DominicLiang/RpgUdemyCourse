using UnityEngine;
using UnityEngine.UI;

public class SkillTreeSlot : MonoBehaviour
{
    [SerializeField] public bool unlocked;
    [SerializeField] private SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private SkillTreeSlot[] shouldBeLocked;
    [SerializeField] private Image skillImage;
    [SerializeField] private string skillName;
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedColor;

    private void OnValidate()
    {
        gameObject.name = $"Skill - {skillName}";
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = lockedColor;

        GetComponent<Button>().onClick.AddListener(() => UnlockSkill());
    }

    public void UnlockSkill()
    {
        foreach (var item in shouldBeUnlocked)
        {
            if (!item.unlocked) return;
        }

        foreach (var item in shouldBeLocked)
        {
            if (item.unlocked) return;
        }

        unlocked = true;
        skillImage.color = Color.white;
    }
}