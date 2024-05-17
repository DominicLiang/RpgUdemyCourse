using UnityEngine;

[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(Damageable))]
public class Player : Character
{
    #region Value
    [Header("Jump Value")]
    public float jumpForce = 20f;
    public int airJumpCount = 1;
    public float swordReturnImpact = 3f;
    public GameObject UsedSword;

    [Header("Dash Value")]
    public float dashSpeed = 25f;
    public float dashDuration = 0.25f;
    public float dashCooldown = 0.4f;
    public int airDashCount = 1;

    [Header("Wallslide Value")]
    public float slideSpeed = 0.5f;
    public float wallJumpXSpeed = 5f;

    [Header("Attack Value")]
    public int comboCount = 3;
    public float comboWindow = 0.25f;
    public float attackSpeed = 1;
    public Vector2[] attackMovement ={
        new Vector2(3,0),
        new Vector2(2,0),
        new Vector2(5,0),
    };

    [Header("Counter Value")]
    public float counterDuration = 0.2f;
    #endregion

    #region Component
    public InputController InputController { get; private set; }
    public Damageable Damageable { get; private set; }
    #endregion

    #region StateMachine
    public IState IdleState { get; private set; }
    public IState MoveState { get; private set; }
    public IState JumpState { get; private set; }
    public IState FallState { get; private set; }
    public IState DashState { get; private set; }
    public IState WallSlideState { get; private set; }
    public IState WallJumpState { get; private set; }
    public IState AttackState { get; private set; }
    public IState HitState { get; private set; }
    public IState DeadState { get; private set; }
    public IState CounterState { get; private set; }
    public IState AimSwordState { get; private set; }
    public IState CatchSwordState { get; private set; }
    #endregion

    protected override void Start()
    {
        base.Start();

        InputController = GetComponent<InputController>();
        Damageable = GetComponent<Damageable>();
        Damageable.onTakeDamage += (from, to) =>
        {
            damageFrom = from;
            Fsm.SwitchState(HitState);
        };

        IdleState = new IdleState(Fsm, this, "Idle");
        MoveState = new MoveState(Fsm, this, "Move");
        JumpState = new JumpState(Fsm, this, "Jump");
        FallState = new FallState(Fsm, this, "Jump");
        DashState = new DashState(Fsm, this, "Dash");
        WallSlideState = new WallSlideState(Fsm, this, "WallSlide");
        WallJumpState = new WallJumpState(Fsm, this, "Jump");
        AttackState = new AttackState(Fsm, this, "Attack");
        HitState = new HitState(Fsm, this, "Hit");
        DeadState = new DeadState(Fsm, this, "Dead");
        CounterState = new CounterState(Fsm, this, "Counter");
        AimSwordState = new AimSwordState(Fsm, this, "AimSword");
        CatchSwordState = new CatchSwordState(Fsm, this, "CatchSword");
        Fsm.SwitchState(IdleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void CatchSword()
    {
        Fsm.SwitchState(CatchSwordState);
        Destroy(UsedSword);
    }
}
