using System.Collections;
using UnityEngine;

public class DeadState : PlayerState
{
    private float dissolveRate = 0.0125f;
    private float refreshRate = 0.025f;

    public DeadState(FSM fsm, Player character, string animBoolName) : base(fsm, character, animBoolName)
    {
    }

    public override void Enter(IState lastState)
    {
        base.Enter(lastState);
        Character.StartCoroutine(Dissolve());
    }

    public override void Update()
    {
        base.Update();
        Rb.velocity = Vector2.zero;
    }

    public override void Exit(IState newState)
    {
        base.Exit(newState);
    }

    IEnumerator Dissolve()
    {
        float counter = 1;
        while (Character.Sr.material.GetFloat("_DissoiveAmount") > 0)
        {
            counter -= dissolveRate;
            Character.Sr.material.SetFloat("_DissoiveAmount", counter);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
