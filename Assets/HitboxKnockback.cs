using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxKnockback : MonoBehaviour
{
    public float time;
    public float magnitude;
    //public string targetTag;

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
        //if (collision.CompareTag(targetTag))
        //{
            //Knockback knockback = collision.GetComponentInParent<Knockback>();
            //collision.GetComponentInParent<Knockback>()?.TakeKnockback(time, magnitude, GetComponent<Collider2D>());
            //Debug.Log("Knocking Back");
        //}

            //Debug.Log(gameObject.GetComponent<Collider2D>().ToString());
        collision.GetComponentInParent<Knockback>()?.TakeKnockback(time, magnitude, GetComponent<Collider2D>());
    }
}
