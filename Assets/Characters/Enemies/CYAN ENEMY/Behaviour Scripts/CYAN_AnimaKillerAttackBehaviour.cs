using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CYAN_AnimaKillerAttackBehaviour : StateMachineBehaviour
{
    private RANGED_enemy enemy;
    private ForceApplier forceApplier;
    private GameObject target;

    [SerializeField] float recoilImpulse;
    [SerializeField] GameObject animaKillerPrefab;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //REFERENCIAS
        enemy = animator.gameObject.GetComponentInParent<RANGED_enemy>();
        target = enemy.target;
        forceApplier = animator.gameObject.GetComponentInParent<ForceApplier>();

        enemy.transform.LookAt(target.transform.position); //Mira a su objetivo

        //-ATTACK ACTION-
        GameObject animaKiller = Instantiate(animaKillerPrefab, new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1.2f, enemy.transform.position.z), enemy.transform.rotation);
        animaKiller.GetComponent<ProjectileController>().target = target.transform;
        //Una vez se han establecido las condiciones para el ataque, se impulsa al enemigo en dirección a su objetivo, con una fuerza igual a 'attackImpulse'
        Vector3 attackDir = (target.transform.position - enemy.transform.position).normalized;
        forceApplier.AddImpact(new Vector3(-attackDir.x, 0, -attackDir.z), recoilImpulse);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isAttacking", false);
    }
}
