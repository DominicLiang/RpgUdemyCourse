using UnityEngine;

public class SKeletonState : CharacterState<Skeleton>, IState
{
    public SKeletonState(FSM fsm, Skeleton character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public virtual void Enter(IState lastState)
    {
        BaseEnter();
    }

    public virtual void Update()
    {
        BaseUpdate();
    }

    public virtual void Exit(IState newState)
    {
        BaseExit();
    }
}
