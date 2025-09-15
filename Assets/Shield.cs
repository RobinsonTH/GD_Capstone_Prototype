using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Collider2D[] colliders;

    [SerializeField] private float time;
    [SerializeField] private float magnitude;
    [SerializeField] List<string> targetTags;

    private void Awake()
    {
        colliders = transform.parent.GetComponentsInChildren<Collider2D>();
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
        

        CollideOnce hitbox = collision.GetComponent<CollideOnce>();
        if (hitbox != null)
        {
            foreach (Collider2D collider in colliders)
            {
                hitbox.Ignore(collider);
            }
        }

        foreach (string tag in targetTags)
        {
            if (collision.CompareTag(tag))
            {
                //Knockback knockback = collision.GetComponentInParent<Knockback>();
                collision.GetComponentInParent<Knockback>()?.TakeKnockback(time, magnitude, GetComponent<Collider2D>());
                return;
            }
        }
    }
}
