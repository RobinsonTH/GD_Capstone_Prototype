using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void DamageHandler(float damage);
    public event DamageHandler OnDamage;

    public delegate void DeathHandler();
    public event DeathHandler OnDeath;

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
            if (OnDamage != null) { OnDamage(damage); }
            if (currentHealth <= 0) { Die(); }
        }
    }    

    public void GainHealth(float healing)
    {
        currentHealth += healing;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void Die()
    {
        if (OnDeath != null) { OnDeath(); }
        gameObject.SetActive(false);
    }
}
