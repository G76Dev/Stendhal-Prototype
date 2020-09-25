using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Behaviour : MonoBehaviour
{
    public Enemy AI;
    [SerializeField] Battle_Trigger eventController;
    private bool onBattle;
    private bool hasEnded;

    // Start is called before the first frame update
    void Start()
    {
        AI.GetComponentInChildren<Animator>().enabled = false;
        AI.enabled = false;
        onBattle = false;
        hasEnded = false;
    }

    public void activateBoss()
    {
        AI.GetComponentInChildren<Animator>().enabled = true;
        AI.enabled = true;
        onBattle = true;
    }

    private void Update()
    {
        if(onBattle)
        {
            if(!AI.isAlive && !hasEnded)
            {
                eventController.endBattle();
                ProtoLevelManager.protoManager.addKey();
                hasEnded = true;
            }
        }
    }
}
