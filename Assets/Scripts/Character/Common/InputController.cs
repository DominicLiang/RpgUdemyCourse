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
    public bool isJumpPressed = false;
    public bool isDashPressed = false;
    public bool isAttackPressed = false;
    public bool isCounterPressed = false;

    private InputAction movementInput;
    private InputAction jumpInput;
    private InputAction dashInput;
    private InputAction attackInput;
    private InputAction counterInput;

    private void Start()
    {
        var input = GetComponent<PlayerInput>();
        movementInput = input.actions.FindAction("Movement", true);
        jumpInput = input.actions.FindAction("Jump", true);
        dashInput = input.actions.FindAction("Dash", true);
        attackInput = input.actions.FindAction("Attack", true);
        counterInput = input.actions.FindAction("Counter", true);
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
        isJumpPressed = jumpInput.IsPressed();
        isDashPressed = dashInput.IsPressed();
        isAttackPressed = attackInput.IsPressed();
        isCounterPressed = counterInput.IsPressed();
    }
}
