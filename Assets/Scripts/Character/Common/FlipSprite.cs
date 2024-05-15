using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    [Header("Flip Sprite")]
    public int facingDir = 1;
    public bool isFacingRight = true;

    public void FlipController(float x)
    {
        if (x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        facingDir = isFacingRight ? 1 : -1;
        transform.Rotate(0, 180, 0);
    }

    [InspectorButton("朝向右的初始值")]
    public void DefaultRight()
    {
        isFacingRight = true;
        facingDir = 1;
    }

    [InspectorButton("朝向左的初始值")]
    public void DefaultLeft()
    {
        isFacingRight = false;
        facingDir = -1;
    }
}
