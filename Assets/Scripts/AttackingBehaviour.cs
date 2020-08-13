using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingBehaviour : StateMachineBehaviour
{
    private Enemy enemy;
    private ForceApplier forceApplier;
    private GameObject target;

    [SerializeField] float attackImpulse;
    [SerializeField] float attackSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.gameObject.GetComponentInParent<Enemy>();
        target = enemy.target;
        forceApplier = animator.gameObject.GetComponentInParent<ForceApplier>();

        enemy.transform.LookAt(enemy.target.transform.position);

        //enemy.agent.SetDestination(enemy.transform.position + (enemy.transform.forward * attackImpulse));
        //enemy.agent.speed = attackSpeed;
        //enemy.agent.velocity = enemy.agent.velocity * 300;

        Vector3 attackDir = (enemy.target.transform.position - enemy.transform.position).normalized;
        forceApplier.AddImpact(attackDir, attackImpulse);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.transform.LookAt(enemy.target.transform.position);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //enemy.agent.velocity = enemy.agent.velocity / 3;
        animator.SetBool("isAttacking", false);
    }
}
