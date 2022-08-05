using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private Slider healthBar;
    [SerializeField] private Health_script Health_obj;

    private void Start()
    {
        healthBar = transform.Find("Health").gameObject.GetComponent<Slider>();
    }

    private void OnEnable()
    {
        Health_obj.healthChangeEvent.AddListener(UpdateHealthBar);
    }

    private void OnDisable()
    {
        Health_obj.healthChangeEvent.RemoveListener(UpdateHealthBar);
    }

    public void UpdateHealthBar(float currentHealth)
    {
        healthBar.value = currentHealth;
    }
}
