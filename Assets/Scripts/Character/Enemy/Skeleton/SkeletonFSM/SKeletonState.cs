using UnityEngine;

public class SkeletonState : CharacterState<Skeleton>, IState
{
    protected static float attackCooldownTimer;

    public SkeletonState(FSM fsm, Skeleton character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public virtual void Enter(IState lastState)
    {
        BaseEnter();
    }

    public virtual void Update()
    {
        BaseUpdate();

        attackCooldownTimer -= Time.deltaTime;
    }

    public virtual void Exit(IState newState)
    {
        BaseExit();
    }
}
