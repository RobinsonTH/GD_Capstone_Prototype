using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyAI : MonoBehaviour
{
    public Rigidbody2D rb;
    private Character character;
    Vector2 moveDirection = Vector2.zero;

    public float moveSpeed;


    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection.x = target.transform.position.x - transform.position.x;
        moveDirection.y = target.transform.position.y - transform.position.y;
        moveDirection.Normalize();
        if (moveDirection != Vector2.zero)
        {
            //transform.rotation = Quaternion.LookRotation(new Vector3 (moveDirection.x, moveDirection.y, 0).normalized);
            transform.rotation = Quaternion.LookRotation(Vector3.back, moveDirection);
        }

        if (character.GetControl())
        {
            if(character.equipped)
            {
                character.FireEquipment();
            }
            else
            {
                rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
            }
                
        }
    }

    
}
