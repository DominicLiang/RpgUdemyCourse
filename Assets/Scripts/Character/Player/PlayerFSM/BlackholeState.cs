
using UnityEngine;

public class BlackholeState : PlayerState
{
    private float flyTime = 0.3f;
    private bool skillUsed;

    public BlackholeState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        skillUsed = false;
        StateTimer = flyTime;
        Rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();

        if (SkillManager.Instance.Blackhole.BlackholeFinished())
        {
            Fsm.SwitchState(Character.FallState);
            return;
        }

        if (StateTimer > 0)
        {
            Rb.velocity = new Vector2(0, 15);
        }
        else
        {
            Rb.velocity = new Vector2(0, -0.1f);
            if (!skillUsed)
            {
                if (!SkillManager.Instance.Blackhole.CanUseSkill()) return;
                skillUsed = true;
            }
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);

        Rb.gravityScale = 1;
        Character.MakeTransprent(false);
    }
}
