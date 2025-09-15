using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHandlerNS : MonoBehaviour
{
    private SpriteRenderer sprite;
    //private Quaternion initialRotation;
    [SerializeField] Sprite north;
    [SerializeField] Sprite south;
    [SerializeField] private float xOffset = 0;
    [SerializeField] private float yOffset = 0;
    
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
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(transform.parent.position.x + xOffset, transform.parent.position.y + yOffset, 0);

        float parentRotation = transform.parent.eulerAngles.z;

        if (state == direction.North && parentRotation >= 90 && parentRotation <= 270)
        {
            ChangeSprite(direction.South);
        }
        else if(state == direction.South && (parentRotation < 90 || parentRotation > 270))
        {
            ChangeSprite(direction.North);
        }
    }

    void ChangeSprite(direction newState)
    {
        state = newState;
        if (state == direction.North) { sprite.sprite = north; }
        else if (state == direction.South) { sprite.sprite = south; }
    }
}
