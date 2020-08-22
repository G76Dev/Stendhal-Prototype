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
    private int currentHealth; //Salud actual del enemigo
    private EnemyHealthBar healthBar;
    public Transform[] patrolSpots;
    [HideInInspector] public int nextSpot;
    public bool isPatrolling;
    public float lookRadius = 10f;
    [Tooltip("Si la distancia que separa al enemigo y al jugador es mayor que esta, el enemigo dejará de perseguirlo")] public float maxChaseDistance = 30f;

    public bool onScreen;
    public bool addedToList;

    [HideInInspector] public int damage;
    [HideInInspector] public float knockback;

    [HideInInspector] public GameObject target;
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
        healthBar = FindObjectOfType<EnemyHealthBar>();

        addedToList = false;

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
            //healthBar.setHealth(currentHealth);

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

    public void visualizeHealth()
    {
        healthBar.setMaxHealth(maxHealth);
        healthBar.setHealth(currentHealth);
    }

    private void Update()
    {
        //GESTIONAR SI EL ENEMIGO ESTÁ EN PANTALLA, Y SI LO ESTÁ, AÑADIRLO A LA LISTA DE ENEMIGOS EN EL COMBAT MANAGER

        //Determinamos la posición relativa del enemigo en el plano de la cámara.
        Vector3 enemyPosition = Camera.main.WorldToViewportPoint(transform.position);

        //Si los valores X e Y del vector anterior están entre 0 y 1, el enemigo se encuentra dentro de la pantalla.
        onScreen = enemyPosition.z > 0 && enemyPosition.x > 0 && enemyPosition.x < 1 && enemyPosition.y > 0 && enemyPosition.y < 1;

        //Finalmente, si el enemigo está dentro de la pantalla, lo añadimos a la lista de enemigos en el manager (una sola vez)
        if(onScreen && !addedToList)
        {
            addedToList = true;
            CombatManager.CM.enemies.Add(this);
        } 
        else if(!onScreen) //Pero si sale de la pantalla, no nos interesa mantenerlo en la lista, de manera que lo quitamos.
        {
            addedToList = false;
            CombatManager.CM.enemies.Remove(this);
        }

    }


    private void Die()
    {
        //Play death animation
        animator.SetTrigger("Death");

        CombatManager.CM.enemies.Remove(this);

        controller.enabled = false; //OJO, hay que modularizar esto para que funcione con cualquier tipo de collider automaticamente.
        this.enabled = false;

        Destroy(gameObject, 10f);
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
