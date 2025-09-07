using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Knockback : MonoBehaviour
{
    public Rigidbody2D rb;
    private Knockback root;
    public float baseKnockback;

    private Coroutine isMoving = null;

    private void Awake()
    {
        root = rb.GetComponent<Knockback>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        if(isMoving != null)
        {
            //Debug.Log(gameObject.ToString() + " was disabled during knockback. -1");
            rb.GetComponent<Character>().GainControl();
            isMoving = null;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void TakeKnockback(float time, float magnitude, Collider2D source)
    {
        //Debug.Log("Starting Coroutine");
        if (gameObject.activeSelf && !GetComponentInParent<Health>().IsInvincible() && !root.IsMoving())
        {
            //Debug.Log(time + " " + magnitude);
            //.Log(gameObject.ToString() + " Getting Knocked Back!");

            Vector2 moveDirection = Vector2.zero;
            //float moveSpeed;
            moveDirection.x = rb.transform.position.x - source.transform.position.x;
            moveDirection.y = rb.transform.position.y - source.transform.position.y;
            moveDirection.Normalize();
            moveDirection *= baseKnockback * magnitude;


            root.SetKnockback(time, moveDirection);
        }
    }

    public void TakeManualKnockback(float time, Vector2 moveVector)
    {
        if (gameObject.activeSelf && !GetComponentInParent<Health>().IsInvincible() && !root.IsMoving())
        {
            moveVector *= baseKnockback;
            root.SetKnockback(time, moveVector);
        }
    }

    private void SetKnockback(float time, Vector2 moveVector)
    {
        isMoving = StartCoroutine(Push(time, moveVector));
    }


    private IEnumerator Push(float time, Vector2 moveVector)
    {
        //Debug.Log(rb.gameObject.ToString() + "was knocked back. +1");
        //lock out other movement scripts
        rb.GetComponent<Character>().LoseControl();

        //set rigidbody trajectory and time based on knockback values

        rb.velocity = moveVector;
        yield return new WaitForSeconds(time);

        //reset trajectory and give back movement control
        rb.velocity = Vector2.zero;
        rb.GetComponent<Character>().GainControl();
        //Debug.Log(rb.gameObject.ToString() + " ended knockback. -1");
        isMoving = null;
    }

    public bool IsMoving()
    {
        return isMoving != null;
    }
}
