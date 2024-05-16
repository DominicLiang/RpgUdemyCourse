using System.Threading.Tasks;
using UnityEngine;

public class PlayerState : CharacterState<Player>
{
    private int airDashCounter;

    protected static float dashCooldownTimer;
    protected static bool isBusy;
    protected float dashDir;
    protected InputController Input { get; private set; }

    public PlayerState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
        Input = character.InputController;
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);

        SetDashDir();
    }

    public override void Update()
    {
        base.Update();

        dashCooldownTimer -= Time.deltaTime;

        Anim.SetFloat("VelocityY", Rb.velocity.y);

        if (ColDetect.IsGrounded)
        {
            airDashCounter = 0;
        }

        if (airDashCounter < Character.airDashCount && dashCooldownTimer <= 0 && !ColDetect.IsWallDetected && Input.isDashDown)
        {
            if (!ColDetect.IsGrounded)
            {
                airDashCounter++;
            }
            SetDashDir();
            Fsm.SwitchState(Character.DashState);
            dashCooldownTimer = Character.dashCooldown;
        }
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    private void SetDashDir()
    {
        dashDir = Input.xAxis == 0 ? Flip.facingDir : Input.xAxis;
    }

    public async void BusyFor(float seconds)
    {
        isBusy = true;
        await Task.Delay((int)(seconds * 1000));
        isBusy = false;
    }
}
