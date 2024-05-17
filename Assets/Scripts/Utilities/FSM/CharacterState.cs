using UnityEngine;

public class CharacterState<T> : IState where T : Character
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

    public virtual void Enter(IState lastState)
    {
        Anim.SetBool(AnimBoolName, true);
        IsAnimationFinished = false;
    }

    public virtual void Update()
    {
        StateTimer -= Time.deltaTime;
    }

    public virtual void Exit(IState newState)
    {
        Anim.SetBool(AnimBoolName, false);
    }

    public void SetVelocity(float x, float y, bool isNeedFlip = true)
    {
        Rb.velocity = new Vector2(x, y);
        if (!isNeedFlip) return;
        Flip.FlipController(x);
    }

    public virtual void AnimationFinishTrigger()
    {
        IsAnimationFinished = true;
    }
}
