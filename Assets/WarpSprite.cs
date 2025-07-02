using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WarpSprite : MonoBehaviour
{
    //public Rigidbody2D rb;
    private float animationTimer;
    string direction;

    //public Vector3 speed;

    // Start is called before the first frame update
    void Start()
    {
        animationTimer = 0;
        //speed = new Vector3(2f, 2f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (animationTimer <= 1 && animationTimer > 0)
        {
            Vector3 speed = Vector3.zero;

            if (direction == "left")
                speed = Vector3.left;
            else if (direction == "right")
                speed = Vector3.right;
            else if (direction == "top")   
                speed = Vector3.up;
            else if (direction == "bottom")
                speed = Vector3.down;


            transform.localPosition += speed * 2 * Time.deltaTime * GetComponentInParent<SpriteRenderer>().bounds.size.x;
        }
        else
        {   
            transform.localPosition = Vector3.zero;
        }

        if(animationTimer > 0)
        {
            animationTimer -= Time.deltaTime;
            if (transform.localPosition.magnitude >= GetComponentInParent<SpriteRenderer>().bounds.size.x)
            {
                if(direction == "left" ||  direction == "right")
                {
                    transform.parent.position = new Vector3(transform.parent.position.x * -1, transform.parent.position.y, 0);
                }
                else if (direction == "top" || direction == "bottom")
                {
                    transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y * -1, 0);
                }
                transform.localPosition = new Vector3(transform.localPosition.x * -1, transform.localPosition.y * -1, 0);
            }
        }
    }

    public void StartAnimation(string d)
    {
        animationTimer = 1.5f;
        direction = d;
        //Debug.Log("Starting Warp Animation");
    }
}
