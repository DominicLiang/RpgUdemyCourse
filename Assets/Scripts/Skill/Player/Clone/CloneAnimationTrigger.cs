using UnityEngine;

public class CloneAnimationTrigger : MonoBehaviour
{
    private CloneSkillController controller;

    void Start()
    {
        controller = GetComponentInParent<CloneSkillController>();
    }

    private void AnimationFinishTrigger()
    {
        controller.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        controller.AttackTrigger();
    }
}
