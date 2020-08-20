using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RED_Patrolling_Behaviour : StateMachineBehaviour
{
    private Enemy enemy;
    private GameObject target;

    private float waitedTime;
    [SerializeField] float waitTime;
    [SerializeField] float wanderingOffset;

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

        if (distance <= enemy.lookRadius)
        {
            animator.SetBool("isChasing", true);
            enemy.isPatrolling = false;
        }

        if (enemy.isPatrolling)
        {
            if (!enemy.agent.hasPath)
            {
                if (waitedTime >= waitTime)
                {
                    enemy.nextSpot = (enemy.nextSpot + 1) % enemy.patrolSpots.Length;
                    enemy.agent.SetDestination(enemy.patrolSpots[enemy.nextSpot].position);
                    waitedTime = 0;
                }
                else
                {
                    waitedTime += Time.deltaTime;
                }
            }
        }
        else //WANDERING
        {
            if (!enemy.agent.hasPath)
            {
                if (waitedTime >= waitTime)
                {
                    Vector3 randomSpot = (Random.insideUnitSphere * wanderingOffset) + enemy.transform.position;
                    Debug.Log(randomSpot);
                    enemy.agent.SetDestination(randomSpot);
                    waitedTime = 0;
                }
                else
                {
                    waitedTime += Time.deltaTime;
                }
            }
        }

        animator.SetFloat("Speed", enemy.agent.velocity.magnitude);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
