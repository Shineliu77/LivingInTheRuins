using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitStateBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        RabbitGM rabbit = animator.GetComponent<RabbitGM>();
        if (rabbit != null)
        {
            rabbit.OnWaitStateEntered();
        }
    }
}
