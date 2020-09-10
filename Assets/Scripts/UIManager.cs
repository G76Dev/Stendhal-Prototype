using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager UIM;

    public GameObject combatHUD;


    public GameObject combatMenuHUD;
    public GameObject selectionMenu, selectionFirstButton, 
        skillsMenu, skillsFirstButton,
        actsMenu, actsFirstButton,
        formsMenu, formsFirstButton,
        itemsMenu, itemsFirstButton;



    private void Awake()
    {
        if (UIM != null) //Si por algún motivo ya existe un combatManager...
        {
            GameObject.Destroy(UIM); //Este script lo mata. Solo puede haber una abeja reina en la colmena.
        }
        else //En caso de que el trono esté libre...
        {
            UIM = this; //Lo toma para ella!
        }

        DontDestroyOnLoad(this); //Ah, y no destruyas esto al cargar
    }

    public void hideAllCombatHUD()
    {
        skillsMenu.SetActive(false);
        formsMenu.SetActive(false);
        itemsMenu.SetActive(false);
        actsMenu.SetActive(false);
        selectionMenu.SetActive(false);
        combatMenuHUD.SetActive(false);
        combatHUD.SetActive(false);
    }

    public void activateCombatMenuHUD()
    {
        skillsMenu.SetActive(false);
        formsMenu.SetActive(false);
        itemsMenu.SetActive(false);
        actsMenu.SetActive(false);
        selectionMenu.SetActive(true);
        combatMenuHUD.SetActive(true);
        combatHUD.SetActive(false);

        //CLEAR selected object
        EventSystem.current.SetSelectedGameObject(null);
        //ASSIGN new selected object
        EventSystem.current.SetSelectedGameObject(selectionFirstButton);
    }

    public void activateCombatHUD()
    {
        combatMenuHUD.SetActive(false);
        combatHUD.SetActive(true);
    }

    public void goToSkills()
    {
        selectionMenu.SetActive(false);
        skillsMenu.SetActive(true);

        //CLEAR selected object
        EventSystem.current.SetSelectedGameObject(null);
        //ASSIGN new selected object
        EventSystem.current.SetSelectedGameObject(skillsFirstButton);
    }

    public void goToActs()
    {
        selectionMenu.SetActive(false);
        actsMenu.SetActive(true);

        //CLEAR selected object
        EventSystem.current.SetSelectedGameObject(null);
        //ASSIGN new selected object
        EventSystem.current.SetSelectedGameObject(actsFirstButton);
    }

    public void goToForms()
    {
        selectionMenu.SetActive(false);
        formsMenu.SetActive(true);

        //CLEAR selected object
        EventSystem.current.SetSelectedGameObject(null);
        //ASSIGN new selected object
        EventSystem.current.SetSelectedGameObject(formsFirstButton);
    }

    public void goToItems()
    {
        selectionMenu.SetActive(false);
        itemsMenu.SetActive(true);

        //CLEAR selected object
        EventSystem.current.SetSelectedGameObject(null);
        //ASSIGN new selected object
        EventSystem.current.SetSelectedGameObject(itemsFirstButton);
    }

    public void returnToSelection()
    {
        skillsMenu.SetActive(false);
        formsMenu.SetActive(false);
        itemsMenu.SetActive(false);
        actsMenu.SetActive(false);
        selectionMenu.SetActive(true);

        //CLEAR selected object
        EventSystem.current.SetSelectedGameObject(null);
        //ASSIGN new selected object
        EventSystem.current.SetSelectedGameObject(selectionFirstButton);
    }


    // Update is called once per frame
    void Update()
    {
       
    }
}
