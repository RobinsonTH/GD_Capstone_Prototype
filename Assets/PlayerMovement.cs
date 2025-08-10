using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject spriteHandler;
    public PlayerInputActions playerControls;

    public float maxHealth;
    public float moveSpeed;
    public float knockbackBaseSpeed;

    Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private InputAction fire;
    private float currentHealth;
    private float damaged; //used to check for recent damage
    private float lostControl; //controls knockback time

    public GameObject warp1;
    public GameObject warp2;
    public GameObject sword;

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
        damaged = -1;
        currentHealth = maxHealth;
        //warp1 = GameObject.Find("WarpEntrance");
        //warp2 = GameObject.Find("WarpExit");
    }

    // Update is called once per frame
    void Update()
    {
        if(lostControl > 0)
        {
            lostControl -= Time.deltaTime;
        }
        else
        {
            lostControl = 0;
            moveSpeed = 4f;
            moveDirection = move.ReadValue<Vector2>();
            if (moveDirection != Vector2.zero)
            {
                //transform.rotation = Quaternion.LookRotation(new Vector3 (moveDirection.x, moveDirection.y, 0).normalized);
                transform.rotation = Quaternion.LookRotation(Vector3.forward, moveDirection.normalized);
            }
        }

        if (damaged >= 0)
        {
            damaged += Time.deltaTime;

            if ((((int)(damaged*60) / 10) % 2 == 0) && !spriteHandler.GetComponent<SpriteRenderer>().enabled)
            {
                spriteHandler.GetComponent<SpriteRenderer>().enabled = true;
            }
            else if ((((int)(damaged*60) / 10) % 2 == 1) && spriteHandler.GetComponent<SpriteRenderer>().enabled)
            {
                spriteHandler.GetComponent<SpriteRenderer>().enabled = false;
            }

            if (damaged >= 1)
            {
                spriteHandler.GetComponent<SpriteRenderer>().enabled = true;
                damaged = -1;
            }
        }
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    /*public void TakeDamage(float damage, bool recoil, Collider2D source)
    {
        if(damaged == -1)
        {
            damaged = 0;
            lostControl = 0.1f;
            if(recoil)
            {
                //set direction vector to be directly away from damage source
                moveDirection.x = transform.position.x - source.transform.position.x;
                moveDirection.y = transform.position.y - source.transform.position.y;
                moveDirection.Normalize();
                moveSpeed = 15f;
            }
        }
    }*/

    public void TakeDamage(float damage, float recoilTime, float recoilMagnitude, Collider2D source)
    {
        if (damaged == -1)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                //gameObject.SetActive(false);
                Die();
                return;
            }

            damaged = 0;

            if (recoilTime > 0)
            {
                lostControl = recoilTime;
                //set direction vector to be directly away from damage source
                moveDirection.x = transform.position.x - source.transform.position.x;
                moveDirection.y = transform.position.y - source.transform.position.y;
                moveDirection.Normalize();
                moveSpeed = knockbackBaseSpeed * recoilMagnitude;
            }
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    void Fire(InputAction.CallbackContext context)
    {
        if (lostControl == 0)
        {
            //Debug.Log(transform.position.x.ToString() + " " + this.GetComponent<boundaries>().getBounds().x.ToString());
            if (Math.Abs(transform.position.x) == (this.GetComponent<boundaries>().getBounds().x - (transform.GetComponent<SpriteRenderer>().bounds.size.x / 2))
                && Math.Abs(transform.position.x + move.ReadValue<Vector2>().x) > (this.GetComponent<boundaries>().getBounds().x - (transform.GetComponent<SpriteRenderer>().bounds.size.x / 2)))
            {
                Debug.Log("Screenwarping Horizontal");
                if (move.ReadValue<Vector2>().x < 0)
                {
                    ScreenWarp("left");
                }
                else
                {
                    ScreenWarp("right");
                }
            }
            else if (Math.Abs(transform.position.y) == (this.GetComponent<boundaries>().getBounds().y - (transform.GetComponent<SpriteRenderer>().bounds.size.y / 2))
                && Math.Abs(transform.position.y + move.ReadValue<Vector2>().y) > (this.GetComponent<boundaries>().getBounds().y - (transform.GetComponent<SpriteRenderer>().bounds.size.y / 2)))
            {
                Debug.Log("Screenwarping Vertical");
                if (move.ReadValue<Vector2>().y < 0)
                {
                    ScreenWarp("bottom");
                }
                else
                {
                    ScreenWarp("top");
                }
            }
            else
            {
                sword.SetActive(true);

            }
        }
    }

    private void ScreenWarp(string direction)
    {
        //x is false
        //y is true
        if (!warp1.activeSelf && !warp2.activeSelf)
        {
            warp1.SetActive(true);
            warp2.SetActive(true);

            lostControl = 1.5f;
            moveDirection = Vector3.zero;

            if (direction == "left" || direction == "right")
            {
                warp1.GetComponent<WarpAnimation>().StartAnimation(new Vector3(this.GetComponent<boundaries>().getBounds().x, transform.position.y, 0));
                warp2.GetComponent<WarpAnimation>().StartAnimation(new Vector3(this.GetComponent<boundaries>().getBounds().x * -1, transform.position.y, 0));
                //transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);

            }
            else
            {
                warp1.GetComponent<WarpAnimation>().StartAnimation(new Vector3(transform.position.x, this.GetComponent<boundaries>().getBounds().y, 0));
                warp2.GetComponent<WarpAnimation>().StartAnimation(new Vector3(transform.position.x, this.GetComponent<boundaries>().getBounds().y * -1, 0));
                //transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
            }
            spriteHandler.GetComponent<WarpSprite>().StartAnimation(direction);

        }
    }
}
