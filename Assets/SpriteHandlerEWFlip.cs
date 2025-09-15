using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHandlerEWFlip : MonoBehaviour
{
    private SpriteRenderer sprite;
    //private Quaternion initialRotation;

    [SerializeField] private float xOffset = 0;
    [SerializeField] private float yOffset = 0;
    

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
        transform.rotation = Quaternion.identity;//initialRotation;
        transform.position = new Vector3(transform.parent.position.x + xOffset, transform.parent.position.y + yOffset, 0);

        float parentRotation = transform.parent.eulerAngles.z;

        if (parentRotation > 0 && parentRotation <= 180)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }
}
