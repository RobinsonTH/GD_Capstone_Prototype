using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    //public GameObject spriteHandler;
    public PlayerInputActions playerControls;

    public float moveSpeed;
    //public float knockbackBaseSpeed;

    Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private InputAction interact;
    private InputAction swing;
    private InputAction fire;
    //private float lostControl; //controls knockback time

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

        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += Interact;

        swing = playerControls.Player.Swing;
        swing.Enable();
        swing.performed += Swing;

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable()
    {
        move.Disable();
        swing.Disable();
        fire.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

        if (GetComponent<Character>().GetControl())
        {
            //moveSpeed = 4f;
            moveDirection = move.ReadValue<Vector2>();
            if (moveDirection != Vector2.zero)
            {
                //transform.rotation = Quaternion.LookRotation(new Vector3 (moveDirection.x, moveDirection.y, 0).normalized);
                transform.rotation = Quaternion.LookRotation(Vector3.forward, moveDirection.normalized);
            }
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }
    }


    

    void Interact(InputAction.CallbackContext context)
    {
        if(GetComponent<DodgeRoll>() != null && moveDirection != Vector2.zero)
        {
            StartCoroutine(GetComponent<DodgeRoll>().Roll());
        }
    }

    void Swing(InputAction.CallbackContext context)
    {
        if (GetComponent<Character>().GetControl())
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

    void Fire(InputAction.CallbackContext context)
    {
        if (GetComponent<Character>().GetControl())
        {
            GetComponent<Character>().FireEquipment();
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

            //lostControl = 1.5f;
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
            //spriteHandler.GetComponent<WarpSprite>().StartAnimation(direction);
            //RE-ENABLE THIS LATER

        }
    }
}
