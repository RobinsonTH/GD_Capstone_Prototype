using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Collider2D[] colliders;

    [SerializeField] private float time;
    [SerializeField] private float magnitude;
    [SerializeField] List<string> targetTags;

    private AudioClip tink;

    private void Awake()
    {
        colliders = transform.parent.GetComponentsInChildren<Collider2D>();
        tink = Resources.Load<AudioClip>("Sounds/fx/select");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        

        foreach (string tag in targetTags)
        {
            if (collision.CompareTag(tag))
            {
                CollideOnce hitbox = collision.GetComponent<CollideOnce>();
                if (hitbox != null)
                {
                    foreach (Collider2D collider in colliders)
                    {
                        hitbox.Ignore(collider);
                        //AudioSource.PlayClipAtPoint(tink, transform.position);
                    }
                    //Debug.Log("Playing audio from CollideOnce Section");
                    //AudioSource.PlayClipAtPoint(tink, transform.position);
                }
                //Knockback knockback = collision.GetComponentInParent<Knockback>();

                //AudioSource.PlayClipAtPoint(tink, transform.position);
                //collision.GetComponentInParent<Knockback>()?.TakeKnockback(time, magnitude, GetComponent<Collider2D>());

                Knockback kb = collision.GetComponentInParent<Knockback>();
                if ((kb != null && kb.TakeKnockback(time, magnitude, GetComponent<Collider2D>())) || kb == null)
                {
                    //Debug.Log("Playing audio from knockback section");
                    AudioSource.PlayClipAtPoint(tink, transform.position);
                }
                return;
            }
        }
    }
}
