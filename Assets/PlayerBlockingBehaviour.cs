using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingBehaviour : StateMachineBehaviour
{
    private CombatController combatController;
    private MovementController movementController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combatController = animator.GetComponentInParent<CombatController>();
        movementController = animator.GetComponentInParent<MovementController>();

        //Mientras el jugador esté en posición de bloqueo, podrá dashear (por eso canDash no se altera), pero no podrá atacar ni moverse.
        combatController.isBlocking = true;
        combatController.canAttack = false;
        movementController.canMove = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Restablecemos los valores.
        combatController.isBlocking = false;
        combatController.canAttack = true;
        movementController.canMove = true;
    }

}
