using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Trigger : MonoBehaviour
{
    [SerializeField] Animator barrierAnimator;
    [SerializeField] Boss_Behaviour enemyBoss;
    private bool hasHappened;

    private void Start()
    {
        hasHappened = false;
    }

    public void endBattle()
    {
        barrierAnimator.SetTrigger("Fall");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !hasHappened)
        {
            barrierAnimator.SetTrigger("Rise");
            enemyBoss.enabled = true;
            enemyBoss.activateBoss();
            enemyBoss.AI.target = other.gameObject;
            hasHappened = true;
        }
    }
}
