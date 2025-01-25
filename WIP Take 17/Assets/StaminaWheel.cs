using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StaminaWheel : MonoBehaviour
{
    float CurrentStamina;
    public float MaxStamina; 
    public float TestMinus;
    public float TestAdd;
    public bool See;
    public GameObject Wheel;
    public GameObject Wheel2;
    public Slider StamWheel;
    public Slider UsageWheel;
    public float UsageValue;

    void Update()
    {
        CurrentStamina = FindObjectOfType<JumpBetter>().Stamina;
        Wheel2.SetActive(See);
        Wheel.SetActive(See);
        if(CurrentStamina >= MaxStamina)
        {
            See = false;
        } else {
            See = true;
            UsageValue = CurrentStamina / MaxStamina + 0.05f;
        }
        StamWheel.value = CurrentStamina / MaxStamina;
        UsageWheel.value = UsageValue;

    }
}
