using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
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
        if(collision.tag == "Player")
        {
            //Debug.Log("Room height before: " + GetComponent<BoxCollider2D>().size.y);

            StartCoroutine(GetComponentInParent<Dungeon>().SetCameraToRoom(this));
            /*Collider2D room = GetComponent<Collider2D>();
            Camera camera = GetComponentInParent<Dungeon>().GetCamera();
            float screenRatio = Screen.height / Screen.width;
            float roomRatio = room.bounds.size.y/room.bounds.size.x;*/
        }

    }
}
