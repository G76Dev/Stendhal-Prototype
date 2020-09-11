using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CYAN_ReadyToAttack_Behaviour : StateMachineBehaviour
{
    private RANGED_enemy enemy;
    private GameObject target;
    private bool doesAnimaKiller;

    private float waitedTime;
    [SerializeField] float attackWaitTime;
    [SerializeField] float specialAttackWaitTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.gameObject.GetComponentInParent<RANGED_enemy>();
        target = enemy.target;

        waitedTime = 0;

        enemy.transform.LookAt(target.transform.position);

        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        //DECISIÓN DE ATAQUE
        //Una vez preparadas las condiciones para atacar, el enemigo decidirá cómo atacar (ataque simple o ataque WP) en base a su condición actual:

        if (enemy.willpower > enemy.animaKillerCost) //Si el enemigo tiene la posibilidad de ejecutar el ataque WP, lo
        {
            //Lo ejecutará
            doesAnimaKiller = true;

            //Y el aviso para hacerlo durará más tiempo que con un ataque normal:
            attackWaitTime = specialAttackWaitTime;

            //ToDo: anuncia ataque con un bark/popup visual
        }
        else //En caso de que no sea posible para él ejecutarlo,
        {
            //Ataca de forma básica,
            doesAnimaKiller = false;
        }


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (waitedTime >= attackWaitTime)
        {
            if (doesAnimaKiller) //Si decidio ejecutar el ataque WP,
            {
                animator.SetTrigger("AnimaKiller"); //lo ejecuta

                enemy.willpower -= enemy.animaKillerCost; //y se actualiza su WP
            }
            else
            {
                //En cualquier otro caso, ejecuta el ataque normal.
                animator.SetBool("isAttacking", true);
            }
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
