using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxKnockback : MonoBehaviour
{
    public float time;
    public float magnitude;
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
        if (collision.CompareTag(targetTag))
        {
           collision.GetComponent<Knockback>().TakeKnockback(time, magnitude, this.GetComponent<Collider2D>());
        }
    }
}
