using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    [SerializeField] Animator animator;
    public GameObject weapon;
    [SerializeField] int maxHealth = 100; //Salud total del enemigo
    public Transform[] patrolSpots;
    [HideInInspector] public int nextSpot;
    public bool isPatrolling;
    public float lookRadius = 10f;
    //private float waitedTime;
    //[SerializeField] float waitTime;


    public Collider attackCollider;
    [HideInInspector] public int damage;
    [HideInInspector] public float knockback;

    public GameObject target;
    private int currentHealth; //Salud actual del enemigo
    private bool vulnerable; //Booleano que determina si el enemigo puede recibir daño en su estado actual.
    private CharacterController controller;
    public NavMeshAgent agent;
    public ForceApplier forceApplier;
    public bool canAttack;
    private bool canDamage;
    [SerializeField] float attackCooldownTime;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        vulnerable = true;
        //waitedTime = waitTime;
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        target = GameObject.FindGameObjectWithTag("Player");
        forceApplier = GetComponent<ForceApplier>();
        canAttack = true;
        canDamage = true;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && canDamage)
        {
            print("el jugador ha recibido " + damage + " PUNTOS DE DAÑO");
            other.GetComponent<CombatController>().takeDamage(damage, knockback, this.transform.forward, this.gameObject);
            canDamage = false; //Ponemos el booleano a false para evitar más de una colisión indeseada
        }
    }

    public IEnumerator cooldownAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownTime);
        canAttack = true;
        canDamage = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
