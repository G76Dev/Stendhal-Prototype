using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    public static CombatManager CM;

    public List<Enemy> enemies;

    public bool onCombat;


    private void Awake()
    {
        if(CM != null) //Si por algún motivo ya existe un combatManager...
        {
            GameObject.Destroy(CM); //Este script lo mata. Solo puede haber una abeja reina en la colmena.
        } 
        else //En caso de que el trono esté libre...
        {
            CM = this; //Lo toma para ella!
        }

        DontDestroyOnLoad(this); //Ah, y no destruyas esto al cargar
    }

    private void Update()
    {
        onCombat = enemies.Count > 0; //Si hay al menos un enemigo en la lista, eso significa que estamos en un combate.
    }


}
