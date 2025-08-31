using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Knockback : MonoBehaviour
{
    public Rigidbody2D rb;
    public float baseKnockback;

    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        isMoving = false;
    }

    private void OnDisable()
    {
        if(isMoving)
        {
            rb.GetComponent<Character>().GainControl();
            isMoving = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void TakeKnockback(float time, float magnitude, Collider2D source)
    {
        //Debug.Log("Starting Coroutine");
        if (gameObject.activeSelf && !GetComponentInParent<Health>().invincible)
        {
            //Debug.Log(time + " " + magnitude);
            StartCoroutine(Push(time, magnitude, source));
        }
    }

    private IEnumerator Push(float time, float magnitude, Collider2D source)
    {
        //Debug.Log("Coroutine in progress");
        //lock out other movement scripts
        rb.GetComponent<Character>().LoseControl();
        isMoving = true;

        //set rigidbody trajectory and time based on knockback values
        Vector2 moveDirection = Vector2.zero;
        float moveSpeed;
        moveDirection.x = rb.transform.position.x - source.transform.position.x;
        moveDirection.y = rb.transform.position.y - source.transform.position.y;
        moveDirection.Normalize();
        moveSpeed = baseKnockback * magnitude;
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        yield return new WaitForSeconds(time);

        //reset trajectory and give back movement control
        rb.velocity = Vector2.zero;
        rb.GetComponent<Character>().GainControl();
        isMoving = false;
    }
}
