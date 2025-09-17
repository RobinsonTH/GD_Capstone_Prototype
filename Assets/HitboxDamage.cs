using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDamage : MonoBehaviour
{
    public int damage;
    public string targetTag;

    private AudioClip hitSound;

    private void Awake()
    {
        hitSound = Resources.Load<AudioClip>("Sounds/fx/thorn");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //when this hitbox collides with an opposing hurtbox with a Health component, deal damage
        if (collision.CompareTag(targetTag))
        {
            


            collision.GetComponentInChildren<SimpleFlash>()?.Flash();

            Health health = collision.GetComponentInParent<Health>();
            if (health != null && health.TakeDamage(damage))
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }

            GetComponent<CollideOnce>()?.Ignore(collision);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //when this hitbox collides with an opposing hurtbox with a Health component, deal damage
        if (collision.CompareTag(targetTag))
        {



            collision.GetComponentInChildren<SimpleFlash>()?.Flash();

            Health health = collision.GetComponentInParent<Health>();
            if (health != null && health.TakeDamage(damage))
            {
                AudioSource.PlayClipAtPoint(hitSound, collision.transform.position);
            }

            GetComponent<CollideOnce>()?.Ignore(collision);
        }
    }
}