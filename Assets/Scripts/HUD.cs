using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private Slider healthBar;

    private void Start()
    {
        healthBar = transform.Find("Health").gameObject.GetComponent<Slider>();
    }

    public void UpdateHealthBar(float currentHealth)
    {
        healthBar.value = currentHealth;
    }
}
