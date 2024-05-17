using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputController : MonoBehaviour
{
    public float xAxis = 0;
    public float yAxis = 0;
    public bool isJumpDown = false;
    public bool isDashDown = false;
    public bool isAttackDown = false;
    public bool isCounterDown = false;
    public bool isAimSwordDown = false;
    public bool isJumpPressed = false;
    public bool isDashPressed = false;
    public bool isAttackPressed = false;
    public bool isCounterPressed = false;
    public bool isAimSwordPressed = false;
    public Vector2 mousePosition = Vector2.zero;

    private InputAction movementInput;
    private InputAction jumpInput;
    private InputAction dashInput;
    private InputAction attackInput;
    private InputAction counterInput;
    private InputAction aimSwordInput;
    private InputAction mouseInput;

    private void Start()
    {
        var input = GetComponent<PlayerInput>();
        movementInput = input.actions.FindAction("Movement", true);
        jumpInput = input.actions.FindAction("Jump", true);
        dashInput = input.actions.FindAction("Dash", true);
        attackInput = input.actions.FindAction("Attack", true);
        counterInput = input.actions.FindAction("Counter", true);
        aimSwordInput = input.actions.FindAction("AimSword", true);
        mouseInput = input.actions.FindAction("Mouse", true);
    }

    private void Update()
    {
        var movement = movementInput.ReadValue<Vector2>();
        xAxis = movement.x;
        yAxis = movement.y;

        isJumpDown = jumpInput.triggered;
        isDashDown = dashInput.triggered;
        isAttackDown = attackInput.triggered;
        isCounterDown = counterInput.triggered;
        isAimSwordDown = aimSwordInput.triggered;
        isJumpPressed = jumpInput.IsPressed();
        isDashPressed = dashInput.IsPressed();
        isAttackPressed = attackInput.IsPressed();
        isCounterPressed = counterInput.IsPressed();
        isAimSwordPressed = aimSwordInput.IsPressed();

        mousePosition = mouseInput.ReadValue<Vector2>();
    }
}
