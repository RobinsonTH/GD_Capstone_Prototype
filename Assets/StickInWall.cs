using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickInWall : MonoBehaviour
{
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
        if(collision.CompareTag("Wall"))
        {
            transform.parent.GetComponent<Character>().LoseControl();
        }
    }
}
