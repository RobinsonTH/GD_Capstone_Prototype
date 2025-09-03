using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteHandler : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Animator animator;
    private Character character;
    private Rigidbody2D rb;
    private Quaternion initialRotation;

    [SerializeField] private float xOffset = 0;
    [SerializeField] private float yOffset = 0;


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        character = GetComponentInParent<Character>();
        rb = GetComponentInParent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.rotation;
    }


    private void Update()
    {


        //Debug.Log(parentRotation);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = initialRotation;
        transform.position = new Vector3(transform.parent.position.x + xOffset, transform.parent.position.y + yOffset, 0);

        


        float parentRotation = transform.parent.eulerAngles.z;

        if(parentRotation > 40 && parentRotation <= 130)
        {
            //left
            animator.SetInteger("WalkDirection", 1);
        }
        else if (parentRotation > 130 && parentRotation <= 220)
        {
            //down
            animator.SetInteger("WalkDirection", 2);
        }
        else if(parentRotation > 220 && parentRotation <= 310)
        {
            //right
            animator.SetInteger("WalkDirection", 3);
        }
        else
        {
            //up
            animator.SetInteger("WalkDirection", 0);
        }

        //Debug.Log("parentrotation = " + parentRotation);

        if (character.GetControl() && rb.velocity != Vector2.zero)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }
}
