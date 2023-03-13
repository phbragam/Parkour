using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlStoppingAction : StateMachineBehaviour
{
    PlayerController player;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if(player == null)
            player = animator.GetComponent<PlayerController>();

        player.HasControl = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        player.HasControl = true;
    }
}
