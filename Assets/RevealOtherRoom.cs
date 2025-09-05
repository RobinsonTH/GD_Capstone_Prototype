using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealOtherRoom : MonoBehaviour
{
    [SerializeField] Room otherRoom;
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
        if (collision.tag == "PlayerOrigin")
        {
            SpriteRenderer roomMask = otherRoom.GetComponent<SpriteRenderer>();
            Color color = roomMask.color;
            color.a = 0f;
            roomMask.color = color;
        }
    }
}
