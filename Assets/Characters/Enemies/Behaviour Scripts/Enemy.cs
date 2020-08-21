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
    [HideInInspector] public int nextSpot;
    public bool isPatrolling;
    public float lookRadius = 10f;


    [HideInInspector] public int damage;
    [HideInInspector] public float knockback;

    [HideInInspector] public GameObject target;
    private int currentHealth; //Salud actual del enemigo
    [HideInInspector] public bool isVulnerable; //Booleano que determina si el enemigo puede recibir daño en su estado actual.
    private CharacterController controller;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public ForceApplier forceApplier;
    [HideInInspector] public bool canAttack;
    private bool canDamage;
    [SerializeField] float attackCooldownTime;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isVulnerable = true;
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

    public void takeDamage(int damage, float knockbackForce, Vector3 knockbackDir, GameObject other)
    {
        if (isVulnerable) //Si el enemigo es vulnerable,
        {
            currentHealth -= damage;
            //Actualizar el HUD que represente la vida de este enemigo

            //Play hurt animation
            animator.SetTrigger("Hurt");

            //Look at the one who attacked
            transform.LookAt(new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z));

            //Apply knockback received from the 'other' who attacks
            if (knockbackForce != 0)
                forceApplier.AddImpact(new Vector3(knockbackDir.x, 0, knockbackDir.z), knockbackForce);

            if (currentHealth <= 0)
            {
                Die(); //Si el HP se reduce por debajo de 0, el enemigo muere.
            }      
        }

    }

    private void Die()
    {
        //Play death animation
        animator.SetTrigger("Death");

        controller.enabled = false; //OJO, hay que modularizar esto para que funcione con cualquier tipo de collider automaticamente.
        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && canDamage)
        {
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
