using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] int damage;
    [SerializeField] float knockback;
    public bool isHoming;
    public bool isPlayerFriendly;
    public float destroyTime;
    public Transform target;
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        if (isHoming && target == null) //Si este proyectil es dirigido pero no tiene objetivo,
        {
            target = GetComponentInParent<ProjectileController>().target; //Intentará copiar el objetivo del objeto padre.
        }


        rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (isHoming && target != null) //Si es un proyectil dirigido, avanza hacia su objetivo
        {
            Vector3 dir = (target.position - this.transform.position).normalized;
            this.transform.LookAt(target);
            rigidbody.velocity = dir * moveSpeed;
        }
        else // Si no es un proyectil dirigido, avanzará hacia delante indefinidamente.
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("COLISION");
        if (other.tag == "Player" && !isPlayerFriendly)
        {
            //print("El jugador recibe " + damage + " puntos de daño!");
            other.GetComponent<CombatController>().takeDamage(damage, knockback, transform.forward, this.gameObject);
            Destroy(gameObject);
        }
        else if (other.tag == "Enemy" && isPlayerFriendly)
        {
            other.GetComponent<Enemy>().takeDamage(damage, knockback, transform.forward, this.gameObject);
            Destroy(gameObject);
        }
        else if (other.tag == "PlayerFriendly")
        {
            //do nothing
        } 
        else
        {
            //Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "PlayerFriendly")
        {
            //do nothing
        } 
        else
        {
        Destroy(gameObject);
        }
    }
}
