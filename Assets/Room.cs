using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerOrigin")
        {
            collision.transform.parent.SetParent(gameObject.transform, true);
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(GetComponentInParent<Dungeon>().SetCameraToRoom(this));
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayerOrigin")
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }

    }
}
