using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{

    public Slider healthSlider;
    public Slider staminaSlider;

    public void setMaxHealth(float health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    public void setHealth(float health)
    {
        healthSlider.value = health;
    }

    public void setMaxStamina(float stamina)
    {
        staminaSlider.maxValue = stamina;
        staminaSlider.value = stamina;
    }

    public void setStamina(float stamina)
    {
        staminaSlider.value = stamina;
    }
}
