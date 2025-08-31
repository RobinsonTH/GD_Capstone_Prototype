using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitboxDestroy : MonoBehaviour
{
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
        if (collision.CompareTag(targetTag) || collision.CompareTag("Environment") || collision.CompareTag("Wall") || collision.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
    }
}
