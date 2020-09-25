using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoLevelManager : MonoBehaviour
{
    public static ProtoLevelManager protoManager;

    [SerializeField] GameObject[] doors;
    public int keys;
    private int keysNeeded = 3;


    // Start is called before the first frame update
    void Start()
    {
        keys = 0;
    }

    private void Awake()
    {
        if (protoManager != null) //Si por algún motivo ya existe un combatManager...
        {
            GameObject.Destroy(protoManager); //Este script lo mata. Solo puede haber una abeja reina en la colmena.
        }
        else //En caso de que el trono esté libre...
        {
            protoManager = this; //Lo toma para ella!
        }
    }

    public void addKey()
    {
        keys++;
        if (keys >= keysNeeded)
            openDoors();
    }

    private void openDoors()
    {
        for(int i = 0; i< doors.Length; i++)
        {
            doors[i].GetComponent<Animator>().SetTrigger("Vanish");
        }
    }

}
