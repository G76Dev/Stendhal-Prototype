using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerHurtBehaviour : StateMachineBehaviour
{
    private CombatController combatController;
    private MovementController movementController;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combatController = animator.GetComponentInParent<CombatController>();
        movementController = animator.GetComponentInParent<MovementController>();

        //Hacemos al jugador invulnerable durante el tiempo que dure la animación de 'recibir daño' y bloqueamos su capacidad de movimiento y ataque.
        combatController.isVulnerable = false;
        combatController.canAttack = false;
        movementController.canDash = false;
        movementController.canMove = false;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Una vez termina la ventana de invulnerabilidad al recibir daño, se le devuelve al jugador la vulnerabilidad y la capacidad de accion
        combatController.isVulnerable = true;
        combatController.canAttack = true;
        movementController.canDash = true;
        movementController.canMove = true;
    }
}
