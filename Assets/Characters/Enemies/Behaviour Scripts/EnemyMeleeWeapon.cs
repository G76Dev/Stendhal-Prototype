using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeWeapon : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float knockback;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            print("El jugador ha recibido " + damage + "puntos de daño!");
            other.GetComponent<CombatController>().takeDamage(damage, knockback, this.transform.up, this.gameObject);
        }
    }

}
