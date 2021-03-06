﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    public static CombatManager CM;

    public List<Enemy> enemies;

    public GameObject combatHUD;

    public bool onCombat;

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
        if (!onCombat)
        {
            combatHUD.SetActive(false);

            //AUDIO
                AudioManager.engine.segmentCode = 0.0f;
            //
        }
        else
        {
            combatHUD.SetActive(true);
            calculateCombatState();
        }
    }

    //AUDIO
    private void calculateCombatState()
    {
        if (player.isDead)
        {
            print("RIP Player");
        }
        else if (EnemiesLowHP())
        {
            print("Enemy LowHP!");
            AudioManager.engine.segmentCode = 3.0f;
        }
        else if (player.health <= (player.maxHealth / 2))
        {
            print("Player LowHP!");
            AudioManager.engine.segmentCode = 2.0f;
        }
        else
        {
            print("Just defaulting over here");
            AudioManager.engine.segmentCode = 1.0f;
        }
    }
    //

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
