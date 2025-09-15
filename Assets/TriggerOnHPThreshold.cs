using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerOnHPThreshold : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private float threshold;
    [SerializeField] private UnityEvent Trigger;
    float lastPercentage = 1;


    private void OnEnable()
    {
        health.OnDamage += CheckThreshold;

    }

    private void OnDisable()
    {
        health.OnDamage -= CheckThreshold;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Trigger == null)
            Trigger = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckThreshold(int _)
    {
        float currentPercentage = ((float)health.currentHealth / health.maxHealth);
        if(lastPercentage > threshold && currentPercentage <= threshold)
        {
            Trigger.Invoke();
        }
        lastPercentage = currentPercentage;
    }
}
