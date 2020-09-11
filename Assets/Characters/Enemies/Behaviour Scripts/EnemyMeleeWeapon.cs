using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeWeapon : MonoBehaviour
{
    [HideInInspector] public int damage;
    [HideInInspector] public float knockback = 30;

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
