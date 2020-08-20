using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] int damage;
    [SerializeField] float knockback;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("COLISION");
        if(other.tag == "Player")
        {
            print("El jugador recibe " + damage + " puntos de daño!");
            other.GetComponent<CombatController>().takeDamage(damage, knockback, transform.forward, this.gameObject);
            Destroy(gameObject);
        } 
        else if(other.tag == "Enemy")
        {
            //do nothing
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
