using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyToAttackBehaviour : StateMachineBehaviour
{
    private Enemy enemy;
    private GameObject target;

    private float waitedTime;
    [SerializeField] float attackWaitTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.gameObject.GetComponentInParent<Enemy>();
        target = enemy.target;

        waitedTime = 0;

        enemy.transform.LookAt(target.transform.position);

        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (waitedTime >= attackWaitTime)
        {
            animator.SetBool("isAttacking", true);
            waitedTime = 0;
        }
        else
        {
            waitedTime += Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        animator.SetBool("isReadyToAttack", false);
        enemy.agent.isStopped = false;
    }

}
