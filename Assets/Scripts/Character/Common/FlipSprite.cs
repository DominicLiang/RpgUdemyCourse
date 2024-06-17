using System;
using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    [Header("Flip Sprite")]
    public int facingDir = 1;
    public bool isFacingRight = true;

    public event Action OnFlip;

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
        OnFlip?.Invoke();
    }
}
