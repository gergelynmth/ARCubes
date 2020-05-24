using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealtBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    public void SetMaxHealt(int healt)
    {
        slider.maxValue = healt;
        slider.value = healt;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealt(int healt)
    {
        slider.value = healt;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
