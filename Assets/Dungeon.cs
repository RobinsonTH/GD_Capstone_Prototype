using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public Camera cam;
    public AnimationCurve lerp;
    public float panDuration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SetCameraToRoom(Room room)
    {
        //calculate aspect ratio of window
        float screenRatio = (float)Screen.height / Screen.width;

        //get real width and height of room regardless of whether the collider was built manually or just scaled, then use those to get H:W ratio of the room
        //float roomWidth = room.transform.localScale.x;// * room.GetComponent<Collider2D>().bounds.size.x;
        //float roomHeight = room.transform.localScale.y;// * room.GetComponent<Collider2D>().bounds.size.y;
        float roomWidth = room.GetComponent<Collider2D>().bounds.size.x;
        float roomHeight = room.GetComponent<Collider2D>().bounds.size.y;
        float roomRatio = roomHeight / roomWidth;

        //cam.transform.position = room.transform.position;


        //acquire target size by determining whether the room's height or width is more extreme compared to the screen's aspect ratio
        float targetSize;
        if (roomRatio >= screenRatio)
        {
            targetSize = roomHeight / 2;
        }
        else
        {
            //if width is more extreme, multiply it by the aspect ratio to get the target height that would encapsulate it
            targetSize = roomWidth * screenRatio / 2;
        }


        //use lerp curve to move and resize camera over panDuration seconds
        float currentTime = 0f;
        float remappedTime = 0f;
        Vector3 startPosition = cam.transform.position;
        float startSize = cam.orthographicSize;
        while (currentTime <= panDuration)
        {
            currentTime += Time.deltaTime;
            remappedTime = lerp.Evaluate(currentTime/panDuration);
            cam.transform.position = Vector3.Lerp(startPosition, room.transform.position, remappedTime);
            cam.orthographicSize = startSize + ((targetSize - startSize) * remappedTime);
            yield return null;
        }
    }
}
