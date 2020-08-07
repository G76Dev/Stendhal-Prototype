using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int maxHealth = 100; //Salud total del enemigo
    private int currentHealth; //Salud actual del enemigo
    private bool vulnerable; //Booleano que determina si el enemigo puede recibir daño en su estado actual.

    private CharacterController controller;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        vulnerable = true;

        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        //controller.detectCollisions = false;
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
        //animator.SetFloat("Speed", agent.speed);
    }
}
