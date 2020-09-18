using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplayer : MonoBehaviour
{
    public EnemyStats stats;

    public Text enemyName;
    public Image enemyType;

    public Slider HPSlider;
    public Image HPFill;
    public Text HPNumber;
    public Slider WPSlider;
    public Image WPFill;
    public Text WPNumber;

    public Image[] weaknesses;
    public Image[] strengths;

    public Text[] WPAttacks;

    // Start is called before the first frame update
    void Start()
    {
        //Asignamos todos los valores de las stats asignadas a los elementos visuales que las representan
        //Nombre y tipo
        enemyName.text = stats.name;
        enemyType.color = stats.animaType;

        //Sliders
        HPSlider.maxValue = stats.health;
        HPNumber.text = stats.health + "/" + HPSlider.maxValue;

        WPSlider.maxValue = stats.willpower;
        WPNumber.text = stats.willpower + "/" + WPSlider.maxValue;

        //Fortalezas y debilidades
        for (int i = 0; i < stats.weaknesses.Length; i++)
        {
            if (stats.weaknesses[i] != null)
            {
                weaknesses[i].color = stats.weaknesses[i];
            }
            else
            {
                weaknesses[i].color = Color.clear;
            }
        }

        for (int i = 0; i < stats.strengths.Length; i++)
        {
            if (stats.strengths[i] != null)
            {
                strengths[i].color = stats.strengths[i];
            }
            else
            {
                strengths[i].color = Color.clear;
            }
        }

        //Ataques de WP
        for (int i = 0; i < stats.willpowerAttacks.Length; i++)
        {
            if (stats.willpowerAttacks[i] != null)
            {
                WPAttacks[i].text = stats.willpowerAttacks[i];
            }
            else
            {
                WPAttacks[i].text = " ";
            }
        }
    }

    public void updateWPandHP(float health, float willpower)
    {
        HPSlider.value = health;
        HPNumber.text = health + "/" + HPSlider.maxValue;
        WPSlider.value = willpower;
        WPNumber.text = Mathf.RoundToInt(willpower) + "/" + WPSlider.maxValue;
    }
}
