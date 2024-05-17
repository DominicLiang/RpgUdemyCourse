using UnityEngine;

public class AimSwordState : PlayerState
{
    public AimSwordState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        SkillManager.Instance.Sword.SetDotsActive(true);
    }

    public override void Update()
    {
        base.Update();

        SetVelocity(0, 0, false);

        if (!Input.isAimSwordPressed)
            Fsm.SwitchState(Character.IdleState);

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Character.transform.position.x > mousePos.x && Flip.facingDir == 1)
        {
            Flip.Flip();
        }
        else if (Character.transform.position.x < mousePos.x && Flip.facingDir == -1)
        {
            Flip.Flip();
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }
}
