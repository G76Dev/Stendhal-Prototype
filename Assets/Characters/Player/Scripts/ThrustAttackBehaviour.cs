using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustAttackBehaviour : MonoBehaviour
{

    [SerializeField] int damage;
    [SerializeField] float knockback;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<Enemy>().takeDamage(damage, knockback, this.transform.forward, this.gameObject);
        }
    }
}
