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
    public bool isJumpPressed = false;
    public bool isDashPressed = false;
    public bool isAttackPressed = false;

    private InputAction movementInput;
    private InputAction jumpInput;
    private InputAction dashInput;
    private InputAction attackInput;

    private void Start()
    {
        var input = GetComponent<PlayerInput>();
        movementInput = input.actions.FindAction("Movement", true);
        jumpInput = input.actions.FindAction("Jump", true);
        dashInput = input.actions.FindAction("Dash", true);
        attackInput = input.actions.FindAction("Attack", true);
    }

    private void Update()
    {
        var movement = movementInput.ReadValue<Vector2>();

        xAxis = movement.x;
        yAxis = movement.y;
        isJumpDown = jumpInput.triggered;
        isDashDown = dashInput.triggered;
        isAttackDown = attackInput.triggered;
        isJumpPressed = jumpInput.IsPressed();
        isDashPressed = dashInput.IsPressed();
        isAttackPressed = attackInput.IsPressed();
    }
}
