using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWillPowerBar : MonoBehaviour
{
    private Slider slider;
    public Gradient gradient;
    public Image fill;
    public Text text;
    [Tooltip("Velocidad a la que la barra se adapta a los nuevos valores que recibe")] [SerializeField] float changingSpeed = 5f;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void setWillpower(float willpower)
    {
        //slider.value = Mathf.Lerp(slider.value, willpower, changingSpeed * Time.deltaTime);
        slider.value = willpower;

        text.text = Mathf.Round(willpower) + "/" + slider.maxValue;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void setMaxWillpower(int willpower)
    {
        slider.maxValue = willpower;

        text.text = willpower + "/" + slider.maxValue;

        fill.color = gradient.Evaluate(1f);
    }
}
