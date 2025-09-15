using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChildOrbit : MonoBehaviour
{
    private SpriteRenderer sprite;
    //private Quaternion initialRotation;
    [SerializeField] string northLayerName;
    [SerializeField] string southLayerName;
    //[SerializeField] private float xOffset = 0;
    //[SerializeField] private float yOffset = 0;

    private enum direction
    {
        North,
        South
    }

    private direction state = direction.South;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //initialRotation = transform.rotation;
    }


    private void Update()
    {


        //Debug.Log(parentRotation);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        //transform.rotation = Quaternion.identity;
        //transform.position = new Vector3(transform.parent.position.x + xOffset, transform.parent.position.y + yOffset, 0);

        float rotation = transform.eulerAngles.z;

        if (state == direction.North && rotation >= 90 && rotation <= 270)
        {
            ChangeLayer(direction.South);
        }
        else if (state == direction.South && (rotation < 90 || rotation > 270))
        {
            ChangeLayer(direction.North);
        }
    }

    void ChangeLayer(direction newState)
    {
        state = newState;
        if (state == direction.North) 
        { 
            sprite.sortingLayerName = northLayerName;
            sprite.flipY = true;
        }
        else if (state == direction.South) 
        {
            sprite.sortingLayerName = southLayerName; 
            sprite.flipY = false;
        }
    }
}
