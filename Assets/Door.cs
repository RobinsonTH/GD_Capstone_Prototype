using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    protected bool locked = false;
    public Collider2D door;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Open())
            {
                GetComponent<SpriteRenderer>().enabled = false;
                door.excludeLayers = LayerMask.GetMask("Player");
            }
        }
    }

    protected virtual bool Open()
    {
        return !locked;
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = true;
            //door.excludeLayers += LayerMask.GetMask("Player");
        }
    }
}
