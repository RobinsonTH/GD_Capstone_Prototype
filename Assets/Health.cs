using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void HealthChangeHandler(int delta);
    public event HealthChangeHandler OnHealthChange;

    public delegate void DamageHandler(int damage);
    public event DamageHandler OnDamage;

    public delegate void DeathHandler();
    public event DeathHandler OnDeath;

    public int maxHealth;
    public int currentHealth;
    public bool invincible;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    


    public void TakeDamage(int damage)
    {
        if (!invincible)
        {
            currentHealth -= damage;
            if (OnDamage != null) { OnDamage(damage); }
            if (OnHealthChange != null) { OnHealthChange(-damage); }
            if (currentHealth <= 0) { Die(); }
        }
    }    

    public void GainHealth(int healing)
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += healing;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            if (OnHealthChange != null) { OnHealthChange(healing); }
        }
        
    }

    public void Die()
    {
        if (OnDeath != null) { OnDeath(); }
        gameObject.SetActive(false);
    }
}
