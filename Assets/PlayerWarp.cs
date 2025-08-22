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

    public GameObject portal;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        hurtbox = GetComponent<Collider2D>();
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Warp(Vector2 targetPosition, Vector2 moveDirection)
    {
        float timer = 0f;
        Vector2 startPosition = transform.position;

        //Fully take over character control and prevent interactions
        character.LoseControl();
        health.invincible = true;
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
        health.invincible = false;
        hurtbox.enabled = true;
        //rb.simulated = true;
        rb.isKinematic = false;
    }

}


