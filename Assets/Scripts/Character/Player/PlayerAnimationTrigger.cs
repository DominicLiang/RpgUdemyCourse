using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void AnimationTrigger()
    {
        var currentState = player.Fsm.currentState as PlayerState;
        currentState?.AnimationFinishTrigger();
    }
}
