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
        moveDirection = move.ReadValue<Vector2>();
        if (moveDirection.magnitude > 0.1f)
        {
            moveDirection.Normalize();

            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg; // Angle in degrees
            float snappedAngle = Mathf.Round(angle / 45f) * 45f; // Snap to nearest 45 degrees

            moveDirection = new Vector2(Mathf.Cos(snappedAngle * Mathf.Deg2Rad), Mathf.Sin(snappedAngle * Mathf.Deg2Rad));
        }
        else
        {
            moveDirection = Vector2.zero;
        }

        if (GetComponent<Character>().GetControl())
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

            if(moveDirection != Vector2.zero)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
            }
        }
    }




    void Interact(InputAction.CallbackContext context)
    {
        if (moveDirection != Vector2.zero && GetComponent<Character>().GetControl())
        {
            if (GetComponent<PlayerWarp>() && transform.parent.GetComponent<Room>() != null)
            {
                Vector2 checkPoint = (Vector2)transform.position + (moveDirection * 0.6f);
                Collider2D wall = Physics2D.OverlapPoint(checkPoint, LayerMask.GetMask("Wall"));
                if (wall != null)
                {
                    
                    //get distance to room bounding box directly across
                    //IntersectRay reverses ray when inside the bounding box in question, so no need to flip signs on moveDirection
                    float distance;
                    if(transform.parent.GetComponent<BoxCollider2D>().bounds.IntersectRay(new Ray(transform.position, moveDirection), out distance))
                    {
                        //Raycast signs work properly, so flip things back the way they should be. Slightly reduce distance to ensure you don't hit the next room over.
                        distance = Math.Abs(distance);
                        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -moveDirection, 0.99f*distance, LayerMask.GetMask("Wall"));

                        //If the raycast returns anything, the final hit is on the wall that we need to warp to
                        //Make sure it's a viable wall and that it's lined up correctly and then move position
                        if (hits.Length > 0 && hits[hits.Length - 1].normal == moveDirection)
                        {
                            //rb.MovePosition(hits[hits.Length - 1].point);
                            StartCoroutine(GetComponent<PlayerWarp>().Warp(hits[hits.Length - 1].point, moveDirection));
                        }
                    }


                }
                else if (GetComponent<DodgeRoll>() != null)
                {
                    StartCoroutine(GetComponent<DodgeRoll>().Roll());
                }
            }
            else if (GetComponent<DodgeRoll>() != null)
            {
                StartCoroutine(GetComponent<DodgeRoll>().Roll());
            }
        }
    }

    void Swing(InputAction.CallbackContext context)
    {
        if (GetComponent<Character>().GetControl())
        {
            //Debug.Log(transform.position.x.ToString() + " " + this.GetComponent<boundaries>().getBounds().x.ToString());
            /*if (Math.Abs(transform.position.x) == (this.GetComponent<boundaries>().getBounds().x - (transform.GetComponent<SpriteRenderer>().bounds.size.x / 2))
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
            {*/
                sword.SetActive(true);

            //}
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
