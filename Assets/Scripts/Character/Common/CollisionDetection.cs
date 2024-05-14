using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(FlipSprite))]
public class CollisionDetection : MonoBehaviour
{
    public LayerMask detectLayerMask;

    [Header("Ground Check")]
    public bool isGrounded = false;
    public float groundCheckDistance = 0.15f;

    [Header("Wall Check")]
    public bool isWallDetected = false;
    public float wallCheckDistance = 0.15f;
    public bool isFullDetected = false;

    private BoxCollider2D col;
    private FlipSprite flip;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        flip = GetComponent<FlipSprite>();
    }

    private void Update()
    {
        isGrounded = GroundCheck();
        isWallDetected = WallCheck(isFullDetected);
    }

    private bool GroundCheck()
    {
        var leftPos = transform.position + new Vector3(-1 * col.size.x / 2, 0, 0);
        var rightPos = transform.position + new Vector3(col.size.x / 2, 0, 0);

        var leftGrounded = Physics2D.Raycast(leftPos, Vector2.down, groundCheckDistance, detectLayerMask);
        var middleGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, detectLayerMask);
        var rightGrounded = Physics2D.Raycast(rightPos, Vector2.down, groundCheckDistance, detectLayerMask);

        return leftGrounded || middleGrounded || rightGrounded;
    }

    private bool WallCheck(bool isFullDetected)
    {
        var facingValue = flip.facingDir;
        var rayDirection = flip.isFacingRight ? Vector2.right : Vector2.left;

        var middlePos = transform.position + new Vector3(col.size.x / 2 * facingValue, col.size.y / 2, 0);
        var middleWallChecked = Physics2D.Raycast(middlePos, rayDirection, wallCheckDistance, detectLayerMask);

        return !isFullDetected ? middleWallChecked : middleWallChecked || FullDetected();

        bool FullDetected()
        {
            var upPos = transform.position + new Vector3(col.size.x / 2 * facingValue, col.size.y, 0);
            var downPos = transform.position + new Vector3(col.size.x / 2 * facingValue, 0.2f, 0);
            var upWallChecked = Physics2D.Raycast(upPos, rayDirection, wallCheckDistance, detectLayerMask);
            var downWallChecked = Physics2D.Raycast(downPos, rayDirection, wallCheckDistance, detectLayerMask);
            return upWallChecked || downWallChecked;
        }
    }
}
