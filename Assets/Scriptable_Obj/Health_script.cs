
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Health_script__", menuName = "Scriptable_obj/Health_script_")]
public class Health_script : ScriptableObject
{
    public float Health = 50f;
    [SerializeField]
    private float MaxHealth = 50f;

    [System.NonSerialized] //??
    public UnityEvent<float> healthChangeEvent;

    private void OnEnable()
    {
        Health = MaxHealth;
        if (healthChangeEvent == null)
        {
            healthChangeEvent = new UnityEvent<float>();
        }
    }

    public void DecreaseHealth(float amount)
    {
        Health -= amount;
        healthChangeEvent.Invoke(Health); //???
    }
}
