using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConcentrationBarController : MonoBehaviour
{
    private Slider slider;
    public Gradient gradient;
    public Image fill;
    [Tooltip("Velocidad a la que la barra se adapta a los nuevos valores que recibe")] [SerializeField] float changingSpeed = 5f;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void setConcentration(float concentration)
    {
        //slider.value = Mathf.Lerp(slider.value, health, changingSpeed * Time.deltaTime);
        slider.value = concentration;

        fill.color = gradient.Evaluate(slider.normalizedValue);

    }

    public void setMaxConcentation(float concentration)
    {
        slider.maxValue = concentration;
        slider.value = concentration;


        fill.color = gradient.Evaluate(1f);
    }
}
