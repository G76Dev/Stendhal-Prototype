using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{

    private Slider slider;
    public Gradient gradient;
    public Image fill;
    [Tooltip("Velocidad a la que la barra se adapta a los nuevos valores que recibe")] [SerializeField] float changingSpeed = 5f;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void setHealth(int health)
    {
        //slider.value = Mathf.Lerp(slider.value, health, changingSpeed * Time.deltaTime);
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);

    }

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;


        fill.color = gradient.Evaluate(1f);
    }
}
