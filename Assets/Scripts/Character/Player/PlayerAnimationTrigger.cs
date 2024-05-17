public class PlayerAnimationTrigger : AnimationTrigger<Player>
{
    private void ThrowSword()
    {
        SkillManager.Instance.Sword.CreateSword();
    }
}
