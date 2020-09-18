using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    public new string name;
    public Color animaType;
    public int health;
    public int willpower;

    public Color[] weaknesses;
    public Color[] strengths;

    public string[] willpowerAttacks;
}
