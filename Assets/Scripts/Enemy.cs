using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int maxHealth = 100; //Salud total del enemigo
    public Transform[] patrolSpots;
    private int nextSpot;
    public bool isPatrolling;
    [SerializeField] float lookRadius = 10f;
    private float waitedTime;
    [SerializeField] float waitTime;

    public GameObject target;
    private int currentHealth; //Salud actual del enemigo
    private bool vulnerable; //Booleano que determina si el enemigo puede recibir daño en su estado actual.
    private CharacterController controller;
    public NavMeshAgent agent;
    public ForceApplier forceApplier;
    public bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        vulnerable = true;
        waitedTime = waitTime;
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        target = GameObject.FindGameObjectWithTag("Player");
        forceApplier = GetComponent<ForceApplier>();
        canAttack = true;

        if (isPatrolling)
        {
            nextSpot = 0;
            agent.SetDestination(patrolSpots[0].position);
        }

    }

    public void takeDamage(int damage)
    {
        print("hit");

        if (vulnerable) //Si el enemigo es vulnerable,
        {
            currentHealth -= damage;
            //print("EL ENEMIGO HA SUFRIDO " + damage + " PUNTOS DE DAÑO");

            //Play hurt animation
            animator.SetTrigger("Hurt");

            if (currentHealth <= 0)
            {
                Die(); //Si el HP se reduce por debajo de 0, el enemigo muere.
            }      
        }

        agent.SetDestination(new Vector3(55, -8, -24));
    }

    private void Die()
    {
        //print("EL ENEMIGO HA MUERTO");
        //Play death animation
        animator.SetBool("isDead", true);

        controller.enabled = false; //OJO, hay que modularizar esto para que funcione con cualquier tipo de collider automaticamente.
        this.enabled = false;
    }


    private void FixedUpdate()
    {

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if(distance <= lookRadius)
        {
            animator.SetBool("isChasing", true);
            isPatrolling = false;
        }

        if(isPatrolling && !agent.hasPath)
        {
            if(waitedTime >= waitTime)
            {
                nextSpot = (nextSpot + 1) % patrolSpots.Length;
                agent.SetDestination(patrolSpots[nextSpot].position);
                waitedTime = 0;
            } 
            else
            {
                waitedTime += Time.deltaTime;
            }

        }

        animator.SetFloat("Speed", agent.speed);
    }

    public IEnumerator cooldownAttack(float sec)
    {
        canAttack = false;
        yield return new WaitForSeconds(sec);
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
