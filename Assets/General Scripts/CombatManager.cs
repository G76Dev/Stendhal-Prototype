using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    public static CombatManager CM;

    public List<Enemy> enemies;

    public GameObject combatHUD;

    public bool onCombat;

    public int combatState;

    private CombatController player;


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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatController>();
    }

    public void showAllStats()
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.showStats();
        }
    }

    public void hideAllStats()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.hideStats();
        }
    }

    private void Update()
    {
        onCombat = enemies.Count > 0; //Si hay al menos un enemigo en la lista, eso significa que estamos en un combate.

        if(!onCombat)
        {
            combatHUD.SetActive(false);
            combatState = -1;
        } 
        else
        {
            combatHUD.SetActive(true);
            calculateCombatState();

        }
    }

    private void calculateCombatState()
    {
        if (player.isDead)
        {
            combatState = 0;
            print("RIP Player");
        }
        else if (player.health <= (player.maxHealth / 4))
        {
            combatState = 1;
            print("Player LowHP!");
        }
        else if (EnemiesLowHP())
        {
            combatState = 2;
            print("Enemy LowHP!");
        }
        else
        {
            combatState = 3;
            print("Just defaulting over here");
        }
    }

    public bool EnemiesLowHP ()
    {
        int totalHP = 0;
        int actualHP = 0;

        foreach (Enemy enemy in enemies)
        {
            totalHP += enemy.stats.health;
        }

        foreach (Enemy enemy in enemies)
        {
            actualHP += enemy.currentHealth;
        }

        if (actualHP <= (totalHP / 4))
        {
            return true;
        } 
        else
        {
            return false;
        }

    }


}
