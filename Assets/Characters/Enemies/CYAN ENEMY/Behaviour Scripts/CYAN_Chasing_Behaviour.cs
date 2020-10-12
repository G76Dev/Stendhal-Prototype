using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CYAN_Chasing_Behaviour : StateMachineBehaviour
{
    private RANGED_enemy enemy;
    private GameObject target;
    [SerializeField] float attackDistance;
    [SerializeField] float fallbackDistance;

    private float waitedTime;
    [SerializeField] float attackCooldown;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.gameObject.GetComponentInParent<RANGED_enemy>();
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

        NavMeshHit hit;

        //Lanza un rayo hacia el objetivo. Si el resultado es false eso significa que no hay ningún obstaculo entre el enemigo y su objetivo.
        if (!enemy.agent.Raycast(target.transform.position, out hit) && fallbackDistance < distance) //Si no hay obstaculos y no está demasiado cerca del jugador...
        {
            if (distance <= attackDistance)
            {
                enemy.agent.isStopped = true;
                enemy.agent.velocity = Vector3.zero;
                enemy.transform.LookAt(target.transform.position);

                if (enemy.canAttack)
                {
                    animator.SetBool("isReadyToAttack", true);
                    enemy.StartCoroutine(enemy.cooldownAttack());
                }
            }
            else
            {
                enemy.agent.isStopped = false;
                enemy.agent.SetDestination(enemy.target.transform.position);
            }
        } 
        else //Si el raycast es true, significa que existe un obstaculo, por lo que el enemigo perseguirá al jugador hasta que ya no haya obstáculos.
        {
            enemy.agent.isStopped = false;
            if (fallbackDistance < distance) //Si el objetivo no está demasiado cerca, el enemigo se aproxima a él.
            {
                enemy.agent.SetDestination(enemy.target.transform.position);
            } 
            else //Si por el contrario el objetivo se ha acercado demasiado, el enemigo comienza a huir hasta haber puesto una distancia de seguridad entre él y su objetivo.
            {
                
                Vector3 dir = enemy.transform.position - target.transform.position;
                enemy.transform.rotation = Quaternion.LookRotation(new Vector3(dir.x,0,dir.z));
                Vector3 aux = enemy.transform.position + (enemy.transform.forward * 2);
                enemy.agent.SetDestination(new Vector3(aux.x,enemy.transform.position.y,aux.z));
            }
        }

        animator.SetFloat("Speed", enemy.agent.velocity.magnitude);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
