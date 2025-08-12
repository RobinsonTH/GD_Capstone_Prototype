using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Knockback : MonoBehaviour
{
    public Rigidbody2D rb;
    public float baseKnockback;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TakeKnockback(float time, float magnitude, Collider2D source)
    {
        //Debug.Log("Starting Coroutine");
        if (gameObject.activeSelf && !GetComponent<Health>().invincible)
        {
            StartCoroutine(Push(time, magnitude, source));
        }
    }

    private IEnumerator Push(float time, float magnitude, Collider2D source)
    {
        //Debug.Log("Coroutine in progress");
        //lock out other movement scripts
        GetComponent<Character>().SetControl(false);

        //set rigidbody trajectory and time based on knockback values
        Vector2 moveDirection = Vector2.zero;
        float moveSpeed;
        moveDirection.x = transform.position.x - source.transform.position.x;
        moveDirection.y = transform.position.y - source.transform.position.y;
        moveDirection.Normalize();
        moveSpeed = baseKnockback * magnitude;
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        yield return new WaitForSeconds(time);

        //reset trajectory and give back movement control
        rb.velocity = Vector2.zero;
        GetComponent<Character>().SetControl(true);
    }
}
