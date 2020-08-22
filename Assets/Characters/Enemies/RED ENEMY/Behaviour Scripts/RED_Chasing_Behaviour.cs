using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RED_Chasing_Behaviour : StateMachineBehaviour
{
    private Enemy enemy;
    private GameObject target;
    [SerializeField] float attackDistance;

    private float waitedTime;
    [SerializeField] float attackCooldown;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.gameObject.GetComponentInParent<Enemy>();
        target = enemy.target;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(enemy.transform.position, target.transform.position);

        if (distance > enemy.maxChaseDistance) //Si el enemigo está demasiado lejos del jugador...
        {
            animator.SetBool("isChasing", false); //Deja de perseguirlo y vuelve a patrullar, o deambular.
            CombatManager.CM.enemies.Remove(enemy); //Quita al enemigo de la lista del CM.
            enemy.addedToList = false; //Y pone este booleano a false para que pueda volver a añadirse si el jugador se vuelve a encontrar con el enemigo.
        }


        if (distance <= attackDistance)
        {
            enemy.agent.isStopped = true;
            enemy.agent.velocity = Vector3.zero;
            enemy.transform.LookAt(target.transform.position);

            if (enemy.canAttack)
            {
                animator.SetBool("isReadyToAttack", true);
            }
        }
        else
        {
            enemy.agent.isStopped = false;
            enemy.agent.SetDestination(enemy.target.transform.position);
        }

        animator.SetFloat("Speed", enemy.agent.velocity.magnitude);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
