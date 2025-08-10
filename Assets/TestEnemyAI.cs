using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyAI : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 moveDirection = Vector2.zero;

    public float moveSpeed = 1f;
    public float maxHealth;
    public float knockbackBaseSpeed;

    private float currentHealth;
    private float damaged; //used to check for recent instances of damage
    private float lostControl; //controls duration of knockback


    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        damaged = -1;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate direction to target by taking the difference of positions
        //moveDirection.x = target.transform.position.x - transform.position.x;
        //moveDirection.y = target.transform.position.y - transform.position.y;

        //Normalize moveDirection vector, then set velocity based on moveSpeed
        //moveDirection.Normalize();
        //rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);


        if (lostControl > 0)
        {
            lostControl -= Time.deltaTime;
        }
        else
        {
            lostControl = 0f;
            moveSpeed = 1f;
            moveDirection.x = target.transform.position.x - transform.position.x;
            moveDirection.y = target.transform.position.y - transform.position.y;
            if (moveDirection != Vector2.zero)
            {
                //transform.rotation = Quaternion.LookRotation(new Vector3 (moveDirection.x, moveDirection.y, 0).normalized);
                transform.rotation = Quaternion.LookRotation(Vector3.back, moveDirection.normalized);
            }
        }

        if (damaged >= 0)
        {
            damaged += Time.deltaTime;

            if (damaged >= 0.1)
            {
                damaged = -1;
            }
        }
        moveDirection.Normalize();
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().TakeDamage(1, 0.1f, 1, this.GetComponent<Collider2D>());
        }
    }

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
                moveSpeed = knockbackBaseSpeed*recoilMagnitude;
            }
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
