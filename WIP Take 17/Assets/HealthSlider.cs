using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    public void UpdateHealthBar(int currentHealth, float maxhealth)
    {
        slider.value = currentHealth / maxhealth;
    }

}
