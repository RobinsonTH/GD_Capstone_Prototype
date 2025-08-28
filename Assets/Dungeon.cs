using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class Dungeon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyHUD;
    
    [SerializeField] Camera mainCam;
    [SerializeField] Camera mapCam;
    public AnimationCurve lerp;
    public float panDuration;
    public int collectedKeys = 0;

    private Room[] rooms;

    // Start is called before the first frame update
    void Start()
    {
        rooms = GetComponentsInChildren<Room>();
    }

    // Update is called once per frame
    void Update()
    {
        keyHUD.text = "× " + collectedKeys.ToString();
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
        Vector3 startPosition = mainCam.transform.position;
        float startSize = mainCam.orthographicSize;
        while (currentTime <= panDuration)
        {
            currentTime += Time.deltaTime;
            remappedTime = lerp.Evaluate(currentTime/panDuration);
            mainCam.transform.position = Vector3.Lerp(startPosition, room.transform.position, remappedTime);
            mainCam.orthographicSize = startSize + ((targetSize - startSize) * remappedTime);
            yield return null;
        }
    }

    public void SetMapCameraToFloor(Floor floor)
    {
        //calculate aspect ratio of window
        float screenRatio = (float)Screen.height / mapCam.ViewportToScreenPoint(new Vector3(1.0f, 0.0f, 0.0f)).x;

        //get real width and height of room regardless of whether the collider was built manually or just scaled, then use those to get H:W ratio of the room
        //float roomWidth = room.transform.localScale.x;// * room.GetComponent<Collider2D>().bounds.size.x;
        //float roomHeight = room.transform.localScale.y;// * room.GetComponent<Collider2D>().bounds.size.y;
        float floorWidth = floor.GetComponent<Collider2D>().bounds.size.x;
        float floorHeight = floor.GetComponent<Collider2D>().bounds.size.y;
        float floorRatio = floorHeight / floorWidth;

        //cam.transform.position = room.transform.position;


        //acquire target size by determining whether the room's height or width is more extreme compared to the screen's aspect ratio
        float targetSize;
        if (floorRatio >= screenRatio)
        {
            targetSize = floorHeight / 2;
        }
        else
        {
            //if width is more extreme, multiply it by the aspect ratio to get the target height that would encapsulate it
            targetSize = floorWidth * screenRatio / 2;
        }
        mapCam.transform.position = floor.transform.position;
        mapCam.orthographicSize = targetSize;

        //use lerp curve to move and resize camera over panDuration seconds
        /*float currentTime = 0f;
        float remappedTime = 0f;
        Vector3 startPosition = mainCam.transform.position;
        float startSize = mainCam.orthographicSize;
        while (currentTime <= panDuration)
        {
            currentTime += Time.deltaTime;
            remappedTime = lerp.Evaluate(currentTime / panDuration);
            mainCam.transform.position = Vector3.Lerp(startPosition, room.transform.position, remappedTime);
            mainCam.orthographicSize = startSize + ((targetSize - startSize) * remappedTime);
            yield return null;
        }*/
    }

    public bool UnlockDoor()
    {
        if(collectedKeys > 0)
        {
            collectedKeys--;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UnmaskExploredRooms()
    {
        foreach (Room room in rooms)
        {
            if(room.IsExplored())
            {
                room.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    public void MaskRooms()
    {
        foreach(Room room in rooms)
        {
            if(!room.IsActive())
            {
                room.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
}
