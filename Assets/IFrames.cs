using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFrames : MonoBehaviour
{
    public float seconds;

    public bool flicker;
    public float flickerSpeed;

    private Health health;
    private SpriteRenderer sprite;

    private void Awake()
    {
        health = GetComponent<Health>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (health != null)
        {
            GetComponent<Health>().OnDamage += InvulnOnHit;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            GetComponent<Health>().OnDamage -= InvulnOnHit;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvulnOnHit(int damage)
    {
        //Debug.Log("Starting Coroutine");
        StartCoroutine(Invincibility());
    }

    private IEnumerator Invincibility()
    {
        //Debug.Log("Coroutine Started");
        health.invincible = true;
        if (flicker)
        {
            float i = 0;
            while (i < seconds)
            {
                health.invincible = true;
                i += flickerSpeed;
                sprite.enabled = !sprite.enabled;
                yield return new WaitForSeconds(flickerSpeed);
            }
            sprite.enabled = true;
        }
        else
        {
            yield return new WaitForSeconds(seconds);
        }
        health.invincible = false;
    }
}
