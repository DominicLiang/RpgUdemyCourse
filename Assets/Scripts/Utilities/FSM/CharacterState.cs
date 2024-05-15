using UnityEngine;

public class CharacterState<T> where T : Character
{
    protected float StateTimer { get; set; }
    protected bool IsAnimationFinished { get; set; }
    protected string AnimBoolName { get; private set; }
    protected FSM Fsm { get; private set; }
    protected T Character { get; private set; }
    protected Animator Anim { get; private set; }
    protected Rigidbody2D Rb { get; private set; }
    protected FlipSprite Flip { get; private set; }
    protected CollisionDetection ColDetect { get; private set; }

    public CharacterState(FSM fsm, T character, string animBoolName)
    {
        AnimBoolName = animBoolName;
        Fsm = fsm;
        Character = character;
        Anim = character.Anim;
        Rb = character.Rb;
        Flip = character.Flip;
        ColDetect = character.ColDetect;
    }

    protected void BaseEnter()
    {
        Anim.SetBool(AnimBoolName, true);
        IsAnimationFinished = false;
    }

    protected void BaseUpdate()
    {
        StateTimer -= Time.deltaTime;
    }
    protected void BaseExit()
    {
        Anim.SetBool(AnimBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        IsAnimationFinished = true;
    }

    public void SetVelocity(float x, float y)
    {
        Rb.velocity = new Vector2(x, y);
        Flip.FlipController(x);
    }
}
