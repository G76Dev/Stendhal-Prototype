using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MELEE_enemy : Enemy
{
    public GameObject weapon;
    public Collider attackCollider;

    [Header("Willpower Attacks", order = 0)]
    [Tooltip("OJO! Este ataque solo lo puede realizar la variante roja de enemigo")] public int tripleChargeCost;
    [Tooltip("OJO! Este ataque solo lo puede realizar la variante violeta de enemigo")] public int swordRondoCost;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
