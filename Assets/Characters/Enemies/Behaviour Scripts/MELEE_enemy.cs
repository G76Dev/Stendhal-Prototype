using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MELEE_enemy : Enemy
{
    public GameObject weapon;
    public Collider attackCollider;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
