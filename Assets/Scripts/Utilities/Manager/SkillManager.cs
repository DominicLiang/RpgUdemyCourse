using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public DashSkill Dash { get; private set; }
    public CloneSkill Clone { get; private set; }
    public SwordSkill Sword { get; private set; }
    public BlackholeSkill blackhole { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;

        Dash = GetComponent<DashSkill>();
        Clone = GetComponent<CloneSkill>();
        Sword = GetComponent<SwordSkill>();
        blackhole = GetComponent<BlackholeSkill>();
    }
}
