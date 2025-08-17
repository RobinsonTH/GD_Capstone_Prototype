using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCameraToRoom(Room room)
    {
        Debug.Log(Screen.width);
        Debug.Log(Screen.height);
        float screenRatio = (float)Screen.height / (float)Screen.width;
        Debug.Log(screenRatio);

        //SpriteRenderer roomBounds = room.GetComponent<SpriteRenderer>();
        //float roomRatio = roomBounds.bounds.size.y / roomBounds.bounds.size.x;
        float roomRatio = room.transform.localScale.y / room.transform.localScale.x;

        cam.transform.position = room.transform.position;

        Debug.Log("Room height after: " + room.transform.localScale.y);

        if (roomRatio >= screenRatio)
        //if(true)
        {
            cam.orthographicSize = room.transform.localScale.y / 2;
            //cam.orthographicSize = 10;
        }
        else
        {
            cam.orthographicSize = room.transform.localScale.x * screenRatio / 2;
            //cam.orthographicSize = 2;
        }
    }
}
