using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxKnockback : MonoBehaviour
{
    public float time;
    public float magnitude;
    //public string targetTag;
    [SerializeField] List<string> targetTags;

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
        Knockback(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Knockback(collision.collider);
    }

    private void Knockback(Collider2D collision)
    {
        foreach (string tag in targetTags)
        {
            if (collision.CompareTag(tag))
            {
                Debug.Log(gameObject.name + " has collided with " + collision.gameObject.name);
                //Knockback knockback = collision.GetComponentInParent<Knockback>();
                collision.GetComponentInParent<Knockback>()?.TakeKnockback(time, magnitude, GetComponent<Collider2D>());
                return;
            }
        }
    }
}
