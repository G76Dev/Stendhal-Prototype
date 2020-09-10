using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatefulArrowBehaviour : StateMachineBehaviour
{

    [SerializeField] GameObject prefab;
    private CombatController playerCC;
    private MovementController playerMC;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerCC = animator.GetComponentInParent<CombatController>();
        playerMC = animator.GetComponentInParent<MovementController>();

        //Restringimos el movimiento y las capacidades del jugador mientras ejecuta el ataque
        playerCC.canAttack = false;
        playerMC.canDash = false;
        playerMC.canMove = false;

        //General el prefab del ataque
        GameObject hatefulArrow = Instantiate(prefab, new Vector3(animator.transform.position.x, animator.transform.position.y + 0.3f, animator.transform.position.z + 1f), Quaternion.identity);
        //Le asigna como objetivo el enemigo fijado por el jugador
        hatefulArrow.GetComponent<ProjectileController>().target = playerCC.lockedEnemy.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerCC.canAttack = true;
        playerMC.canDash = true;
        playerMC.canMove = true;
    }

}
