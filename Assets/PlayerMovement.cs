using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    //public PlayerInputActions playerControls;

    public float moveSpeed;

    Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private InputAction interact;
    private InputAction swing;
    private InputAction fire;
    private InputAction block;

    //public GameObject warp1;
    //public GameObject warp2;
    public GameObject sword;
    [SerializeField] private GameObject shield;

    private Vector2 startPosition;

    private void Awake()
    {
        //playerControls = new PlayerInputActions();
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        transform.position = startPosition;

        move = InputManager.Actions.Player.Move;
        move.Enable();

        interact = InputManager.Actions.Player.Interact;
        interact.Enable();
        interact.performed += Interact;

        swing = InputManager.Actions.Player.Swing;
        swing.Enable();
        swing.performed += Swing;

        fire = InputManager.Actions.Player.Fire;
        fire.Enable();
        fire.performed += Fire;

        block = InputManager.Actions.Player.Block;
        block.Enable();
        block.performed += RaiseShield;
        //block.canceled += DropShield;

    }

    private void OnDisable()
    {
        move.Disable();
        swing.Disable();
        fire.Disable();
        block.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        if (!sword.activeSelf)
        {
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
                if (!shield.activeSelf)
                {
                    rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
                }

                if (moveDirection != Vector2.zero)
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
                }
            }
        }
    }




    void Interact(InputAction.CallbackContext context)
    {
        if (moveDirection != Vector2.zero && GetComponent<Character>().GetControl() && !shield.activeSelf && !sword.activeSelf)
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
        if (GetComponent<Character>().GetControl() && !shield.activeSelf && !sword.activeSelf)
        {
            rb.velocity = Vector3.zero;
            sword.SetActive(true);
        }
    }

    void Fire(InputAction.CallbackContext context)
    {
        if (GetComponent<Character>().GetControl() && !shield.activeSelf && !sword.activeSelf)
        {
            GetComponent<Character>().FireEquipment();
        }
    }

    void RaiseShield(InputAction.CallbackContext context)
    {
        if (GetComponent<Character>().GetControl())
        {
            rb.velocity = Vector3.zero;
            shield.SetActive(true);
            block.canceled += DropShield;
            GetComponent<Health>().OnDamage += DropShield;
        }
    }

    void DropShield(InputAction.CallbackContext context)
    {
        DropShield(0);
    }

    void DropShield(int damage)
    {
        if(shield.activeSelf)
        {
            shield.SetActive(false);
            block.canceled -= DropShield;
            GetComponent<Health>().OnDamage -= DropShield;
        }
    }
}
