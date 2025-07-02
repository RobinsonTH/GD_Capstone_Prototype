using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 4f;
    public PlayerInputActions playerControls;

    Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private InputAction fire;
    private int damaged;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();

        fire.performed += Fire;
    }

    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        damaged = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (damaged == 0)
        {
            moveDirection = move.ReadValue<Vector2>();
        }
        else if (damaged++ >= 6)
        {
            moveDirection = move.ReadValue<Vector2>();
            moveSpeed = 4f;

            if(((damaged/10) % 2 == 0) && !this.GetComponent<Renderer>().enabled)
            {
                this.GetComponent<Renderer>().enabled = true;
            }
            else if(((damaged/10) % 2 == 1) && this.GetComponent<Renderer>().enabled)
            {
                this.GetComponent<Renderer>().enabled = false;
            }


            if (damaged >= 60)
            {
                this.GetComponent<Renderer>().enabled = true;
                damaged = 0;
            }
        }
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    public void TakeDamage(int damage, bool recoil, Collider2D source)
    {
        if(damaged == 0)
        {
            damaged = 1;
            if(recoil)
            {
                //set direction vector to be directly away from damage source
                moveDirection.x = transform.position.x - source.transform.position.x;
                moveDirection.y = transform.position.y - source.transform.position.y;
                moveDirection.Normalize();
                moveSpeed = 15f;
            }
        }
    }

    void Fire(InputAction.CallbackContext context)
    {
        //Debug.Log(transform.position.x.ToString() + " " + this.GetComponent<boundaries>().getBounds().x.ToString());
        if(Math.Abs(transform.position.x) == (this.GetComponent<boundaries>().getBounds().x - (transform.GetComponent<SpriteRenderer>().bounds.size.x / 2))
            && Math.Abs(transform.position.x + move.ReadValue<Vector2>().x) > (this.GetComponent<boundaries>().getBounds().x - (transform.GetComponent<SpriteRenderer>().bounds.size.x / 2)))
        {
            //Debug.Log("Screenwarping");
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);
        }
        else if(Math.Abs(transform.position.y) == (this.GetComponent<boundaries>().getBounds().y - (transform.GetComponent<SpriteRenderer>().bounds.size.y / 2))
            && Math.Abs(transform.position.y + move.ReadValue<Vector2>().y) > (this.GetComponent<boundaries>().getBounds().y - (transform.GetComponent<SpriteRenderer>().bounds.size.y / 2)))
        {
            //Debug.Log("Screenwarping");
            transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
        }
    }
}
