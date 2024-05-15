using UnityEngine;

public class HitState : PlayerState
{
    public HitState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
    }

    public override void Update()
    {
        base.Update();
        Debug.Log(Character.damageFrom);
        if (Character.damageFrom)
        {
            var isRight = Character.damageFrom.transform.position.x > Character.transform.position.x;
            var isLeft = Character.damageFrom.transform.position.x < Character.transform.position.x;
            var moveDir = isRight ? 1 : isLeft ? -1 : 0;
            SetVelocity(moveDir * -1 * Character.knockbackXSpeed, Character.knockbackYSpeed, false);
        }

        if (IsAnimationFinished)
            Fsm.SwitchState(Character.IdleState);
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
