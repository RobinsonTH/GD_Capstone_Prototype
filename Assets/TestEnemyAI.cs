using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyAI : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Rigidbody2D rb;
    Vector2 moveDirection = Vector2.zero;


    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Calculate direction to target by taking the difference of positions
        moveDirection.x = target.transform.position.x - transform.position.x;
        moveDirection.y = target.transform.position.y - transform.position.y;

        //Normalize moveDirection vector, then set velocity based on moveSpeed
        moveDirection.Normalize();
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().TakeDamage(1, true, this.GetComponent<Collider2D>());
        }
    }
}
