using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void DamageHandler(float damage);
    public event DamageHandler onDamage;

    public float maxHealth;
    public float currentHealth;
    public bool invincible;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TakeDamage(float damage)
    {
        if (!invincible)
        {
            currentHealth -= damage;
            if (onDamage != null) { onDamage(damage); }
            if (currentHealth <= 0) { Die(); }
        }
    }    



    public void Die()
    {
        gameObject.SetActive(false);
    }
}
