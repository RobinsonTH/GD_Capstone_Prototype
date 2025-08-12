using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFrames : MonoBehaviour
{
    public float seconds;

    public bool flicker;
    public float flickerSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Health>())
        {
            GetComponent<Health>().onDamage += InvulnOnHit;
        }
    }

    private void OnDisable()
    {
        if (GetComponent<Health>())
        {
            GetComponent<Health>().onDamage -= InvulnOnHit;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvulnOnHit(float damage)
    {
        //Debug.Log("Starting Coroutine");
        StartCoroutine(Invincibility());
    }

    private IEnumerator Invincibility()
    {
        //Debug.Log("Coroutine Started");
        GetComponent<Health>().invincible = true;
        if (flicker)
        {
            float i = 0;
            while (i < seconds)
            {
                i += flickerSpeed;
                GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
                yield return new WaitForSeconds(flickerSpeed);
            }
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            yield return new WaitForSeconds(seconds);
        }
        GetComponent<Health>().invincible = false;
    }
}
