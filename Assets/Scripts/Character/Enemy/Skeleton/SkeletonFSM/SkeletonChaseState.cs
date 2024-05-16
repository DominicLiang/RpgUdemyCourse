using UnityEngine;

public class SkeletonChaseState : SkeletonGroundState
{
    public SkeletonChaseState(FSM fsm, Skeleton character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        StateTimer = Character.lostPlayerTime;
    }

    public override void Update()
    {
        base.Update();

        if (!ColDetect.DetectedPlayer)
        {
            Fsm.SwitchState(Character.IdleState);
            return;
        }

        var isRight = ColDetect.DetectedPlayer.position.x > Character.transform.position.x;
        var isLeft = ColDetect.DetectedPlayer.position.x < Character.transform.position.x;
        var moveDir = isRight ? 1 : isLeft ? -1 : 0;

        var distance = Vector2.Distance(ColDetect.DetectedPlayer.position, Character.transform.position);
        SetVelocity(moveDir * Character.moveSpeed, Rb.velocity.y);

        if (attackCooldownTimer < 0 && distance < Character.attackDistance)
        {
            Fsm.SwitchState(Character.AttackState);
        }

        if (StateTimer < 0 || distance - 1 > ColDetect.playerCheckDistance)
        {
            ColDetect.DetectedPlayer = null;
            Fsm.SwitchState(Character.IdleState);
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
