using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CYAN_Attack_Behaviour : StateMachineBehaviour
{
    private RANGED_enemy enemy;
    private ForceApplier forceApplier;
    private GameObject target;

    [SerializeField] float attackImpulse;
    [SerializeField] float attackSpeed;
    [SerializeField] float attackDamage;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //REFERENCIAS
        enemy = animator.gameObject.GetComponentInParent<RANGED_enemy>();
        target = enemy.target;
        forceApplier = animator.gameObject.GetComponentInParent<ForceApplier>();

        enemy.transform.LookAt(target.transform.position);

        //-ATTACK SETUP-
        //Ignora la colisión entre el enemigo y el objetivo del ataque para que no obstaculice la embestida
        Physics.IgnoreCollision(target.GetComponent<CharacterController>(), enemy.GetComponent<CharacterController>(), true);
        //Acto seguido, activa el trigger que es el que se encargará de gestionar si el ataque ha golpeado, su daño, knockback, etc.
        //enemy.attackCollider.enabled = true;
        //Y finalmente reemplazamos el daño 'default' por el daño de este ataque en concreto.
        enemy.damage = attackDamage;

        //-ATTACK ACTION-
        Instantiate(enemy.projectile, new Vector3(enemy.transform.position.x,enemy.transform.position.y + 1.2f, enemy.transform.position.z), enemy.transform.rotation);
        //Una vez se han establecido las condiciones para el ataque, se impulsa al enemigo en dirección a su objetivo, con una fuerza igual a 'attackImpulse'
        Vector3 attackDir = (target.transform.position - enemy.transform.position).normalized;
        forceApplier.AddImpact(new Vector3(-attackDir.x, 0, -attackDir.z), attackImpulse);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Una vez se ha completado el ataque, levantamos todas las condiciones especiales que se dieron para este ataque.
        animator.SetBool("isAttacking", false);
        Physics.IgnoreCollision(target.GetComponent<CharacterController>(), enemy.GetComponent<CharacterController>(), false);
        //enemy.attackCollider.enabled = false;
    }
}
