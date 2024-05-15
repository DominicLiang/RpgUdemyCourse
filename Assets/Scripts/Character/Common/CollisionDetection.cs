using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(FlipSprite))]
public class CollisionDetection : MonoBehaviour
{
    [Header("Platform Check")]
    public LayerMask platformLayer;
    public bool IsGrounded = false;
    public float groundCheckDistance = 0.15f;
    public bool IsWallDetected = false;
    public float wallCheckDistance = 0.15f;
    public bool isFullDetected = false;

    [Header("Player Check")]
    public LayerMask playerLayer;
    public Transform DetectedPlayer;
    public float playerCheckDistance = 15f;
    public float playerCheckBackDistance = 5f;
    public bool isNeedPlayerCheck = false;

    private BoxCollider2D col;
    private FlipSprite flip;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        flip = GetComponent<FlipSprite>();
    }

    private void Update()
    {
        GroundCheck();
        WallCheck(isFullDetected);
        if (!isNeedPlayerCheck) return;
        PlayerCheck();
    }

    private void GroundCheck()
    {
        var leftPos = transform.position + new Vector3(-1 * col.size.x / 2, 0, 0);
        var rightPos = transform.position + new Vector3(col.size.x / 2, 0, 0);

        var leftGrounded = Physics2D.Raycast(leftPos, Vector2.down, groundCheckDistance, platformLayer);
        var middleGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, platformLayer);
        var rightGrounded = Physics2D.Raycast(rightPos, Vector2.down, groundCheckDistance, platformLayer);

        IsGrounded = leftGrounded || middleGrounded || rightGrounded;
    }

    private void WallCheck(bool isFullDetected)
    {
        var facingValue = flip.facingDir;
        var rayDirection = flip.isFacingRight ? Vector2.right : Vector2.left;

        var middlePos = transform.position + new Vector3(col.size.x / 2 * facingValue, col.size.y / 2, 0);
        var middleWallChecked = Physics2D.Raycast(middlePos, rayDirection, wallCheckDistance, platformLayer);

        IsWallDetected = !isFullDetected ? middleWallChecked : middleWallChecked || FullDetected();

        bool FullDetected()
        {
            var upPos = transform.position + new Vector3(col.size.x / 2 * facingValue, col.size.y, 0);
            var downPos = transform.position + new Vector3(col.size.x / 2 * facingValue, 0.2f, 0);
            var upWallChecked = Physics2D.Raycast(upPos, rayDirection, wallCheckDistance, platformLayer);
            var downWallChecked = Physics2D.Raycast(downPos, rayDirection, wallCheckDistance, platformLayer);
            return upWallChecked || downWallChecked;
        }
    }

    private void PlayerCheck()
    {
        var pos = transform.position + new Vector3(0, col.size.y / 2, 0);
        var rayDirection = flip.isFacingRight ? Vector2.right : Vector2.left;
        var hit = Physics2D.Raycast(pos, rayDirection, playerCheckDistance, playerLayer);
        var hitBack = Physics2D.Raycast(pos, rayDirection * -1, playerCheckBackDistance, playerLayer);
        if (!hit && !hitBack) return;
        DetectedPlayer = hit ? hit.collider.transform : hitBack.collider.transform;
    }
}
