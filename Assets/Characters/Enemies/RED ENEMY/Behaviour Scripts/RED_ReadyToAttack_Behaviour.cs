using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RED_ReadyToAttack_Behaviour : StateMachineBehaviour
{
    private MELEE_enemy enemy;
    private GameObject target;
    private bool doesTripleCharge;
    private GameObject bark;

    private float waitedTime;
    [SerializeField] float attackWaitTime;
    [SerializeField] float specialAttackWaitTime;
    [SerializeField] GameObject barkAttackPrefab;
    [SerializeField] string specialAttackName;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.gameObject.GetComponentInParent<MELEE_enemy>();
        target = enemy.target;



        waitedTime = 0;

        //Hacer que el enemigo mire hacia su objetivo
        enemy.transform.LookAt(target.transform.position);
        enemy.isVulnerable = false; //Hacerlo invulnerable durante la ventana de tiempo en la que 'anuncia' su ataque.

        //Detener en seco al enemigo
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        //DECISIÓN DE ATAQUE
        //Una vez preparadas las condiciones para atacar, el enemigo decidirá cómo atacar (ataque simple o ataque WP) en base a su condición actual:

        if(enemy.willpower > enemy.tripleChargeCost) //Si el enemigo tiene la posibilidad de ejecutar el ataque WP, lo
        {
            //Lo ejecutará
            doesTripleCharge = true;

            //Y el aviso para hacerlo durará más tiempo que con un ataque normal:
            attackWaitTime = specialAttackWaitTime;

            //Anuncia ataque con un bark/popup visual !!!1
            var canvas = FindObjectOfType<Canvas>();
            Vector3 viewportPosition = Camera.main.WorldToScreenPoint(new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2, enemy.transform.position.z));

            bark = Instantiate(barkAttackPrefab, viewportPosition, Quaternion.identity);
            bark.GetComponentInChildren<Text>().text = specialAttackName;
            bark.transform.SetParent(canvas.transform, false);
            bark.transform.position = new Vector3(viewportPosition.x, viewportPosition.y, viewportPosition.z);
        } 
        else //En caso de que no sea posible para él ejecutarlo,
        {
            //Ataca de forma básica,
            doesTripleCharge = false;
        }


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (waitedTime >= attackWaitTime)
        {
            if(doesTripleCharge) //Si decidio ejecutar el ataque WP,
            {
                animator.SetTrigger("TripleCharge"); //lo ejecuta

                enemy.willpower -= enemy.tripleChargeCost; //y se actualiza su WP
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
        //Hacemos que el bark se mantenga en su sitio mientras tanto.
        bark.transform.position = Camera.main.WorldToScreenPoint(new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2, enemy.transform.position.z));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        animator.SetBool("isReadyToAttack", false);
        enemy.agent.isStopped = false;
        Destroy(bark, 0.2f); //Al ejecutar el ataque, destruye el 'bark' que lo anunciaba porque ya no es necesario.
    }

}
