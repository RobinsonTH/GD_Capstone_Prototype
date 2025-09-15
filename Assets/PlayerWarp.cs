using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarp : MonoBehaviour
{
    public float duration = 2.0f; //should be the same as WarpPortal animation duration
    private Health health;
    private Collider2D hurtbox;
    private Character character;
    private Rigidbody2D rb;

    public GameObject portal = null;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        hurtbox = GetComponent<Collider2D>();
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();

        if(portal == null)
        {
            portal = Resources.Load<GameObject>("Objects/WarpPortal");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool WarpPlayer(Vector2 moveDirection)
    {
        if (transform.parent.GetComponent<Room>() != null)
        {
            Vector2 checkPoint = (Vector2)transform.position + (moveDirection * 0.4f);
            Collider2D wall = Physics2D.OverlapPoint(checkPoint, LayerMask.GetMask("Wall"));
            if (wall != null)
            {

                //get distance to room bounding box directly across
                //IntersectRay reverses ray when inside the bounding box in question, so no need to flip signs on moveDirection
                float distance;
                if (transform.parent.GetComponent<BoxCollider2D>().bounds.IntersectRay(new Ray(transform.position, moveDirection), out distance))
                {
                    //Raycast signs work properly, so flip things back the way they should be. Slightly reduce distance to ensure you don't hit the next room over.
                    distance = Math.Abs(distance);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -moveDirection, 0.99f * distance, LayerMask.GetMask("Wall"));

                    //If the raycast returns anything, the final hit is on the wall that we need to warp to
                    //Make sure it's a viable wall and that it's lined up correctly and then move position
                    if (hits.Length > 0)// && hits[hits.Length - 1].normal == moveDirection)
                    //if (hits.Length > 0 && hits[0].normal == moveDirection)
                    {
                        int hit = hits.Length - 1;
                        while(hit >= 0
                            && (hits[hit].normal != moveDirection
                            || Physics2D.OverlapPoint((hits[hit].point + 0.99f * moveDirection), LayerMask.GetMask("Wall")) != null))
                        {
                            hit--;
                        }
                        if(hit < 0)
                        {
                            return false;
                        }
                        StartCoroutine(GetComponent<PlayerWarp>().Warp(hits[hit].point, moveDirection));
                        return true;
                    }
                }
            }
        }
        return false;
    }


    private IEnumerator Warp(Vector2 targetPosition, Vector2 moveDirection)
    {
        float timer = 0f;
        Vector2 startPosition = transform.position;

        //Fully take over character control and prevent interactions
        character.LoseControl();
        health.GainInvincibility();
        hurtbox.enabled = false;
        //rb.simulated = false;
        rb.isKinematic = true;

        //Instantiate two portal objects at start and end points
        Instantiate(portal, ((Vector3)transform.position + (Vector3)(0.5f * moveDirection)), transform.rotation); //close portal
        Instantiate(portal, targetPosition, (transform.rotation * Quaternion.Euler(0, 0, 180f))); //far portal

        //Wait while portal grows
        yield return new WaitForSeconds(0.25f * duration);

        //Enter portal
        while(timer <= 0.25f * duration)
        {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(startPosition, (startPosition + moveDirection), (timer / (0.25f * duration)));
            yield return null;
        }

        //Teleport, then reset variables
        transform.position = targetPosition;

        timer = 0f;
        startPosition = targetPosition - 0.5f * moveDirection;


        //Exit portal
        while (timer <= 0.25f * duration)
        {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(startPosition, (startPosition + moveDirection), (timer / (0.25f * duration)));
            yield return null;
        }


        //Wait while portal shrinks
        yield return new WaitForSeconds(0.25f * duration);




        //Give back all control
        character.GainControl();
        health.LoseInvincibility();
        hurtbox.enabled = true;
        //rb.simulated = true;
        rb.isKinematic = false;
    }

}


