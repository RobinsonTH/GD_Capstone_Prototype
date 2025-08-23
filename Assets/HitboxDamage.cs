using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDamage : MonoBehaviour
{
    public int damage;
    public string targetTag;

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
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}