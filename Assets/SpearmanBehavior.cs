using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class SpearmanBehavior : MonoBehaviour
{

    private Character character;
    private Rigidbody2D rb;
    private NavMeshAgent agent;
    [SerializeField] private Transform target;
    //[SerializeField] private AnimationCurve dashCurve;
    //[SerializeField] private float dashSpeed;

    private bool isIdle = true;
    private IEnumerator coroutine;
    private enum State
    {
        /*Move,
        AcquireTarget,
        Fire,
        RatScurry,
        RandomDash,
        TurnRadius,*/
        HuntTarget,
        Fire,
        Wait
    }

    [SerializeField] private State startState;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxRotation;
    private State state = State.Wait;
    private int iterator = 3;

    private void Awake()
    {
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.updatePosition = false;
    }

    private void OnEnable()
    {
        state = startState;
        iterator = 1;
        isIdle = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isIdle && character.GetControl())
        {
            switch (state)
            {
                /*case State.Move:
                    break;
                case State.AcquireTarget:
                    coroutine = AcquireTarget(0.1f);
                    state = State.Fire;
                    iterator = 0;
                    break;*/
                case State.HuntTarget:
                    coroutine = HuntTarget();
                    state = State.Fire;
                    iterator = 1;
                    break;
                case State.Fire:
                    coroutine = character.equipped.Fire(character);
                    if (iterator <= 0) 
                    { 
                        state = State.HuntTarget;
                        iterator = 1;
                    }
                    break;
                /*case State.RatScurry:
                    coroutine = RatScurry();
                    if(iterator <= 0)
                    {
                        state = State.Wait;
                        iterator = 1;
                    }
                    break;
                case State.RandomDash:
                    coroutine = RandomDash(dashSpeed, 0.5f);
                    if (iterator >= 3)
                    {
                        state = State.AcquireTarget;
                        iterator = 0;
                    }
                    break;*/
                case State.Wait:
                    coroutine = Wait(1.5f);
                    state = State.HuntTarget;
                    iterator = 1;
                    break;
                default:
                    break;
            }
            //Debug.Log("State Machine reached a decision. Iterator = " + iterator);
            //Debug.Log("Current State is " + state.ToString());
            StartCoroutine(WrapperCoroutine(coroutine));
        }
    }

    private IEnumerator WrapperCoroutine(IEnumerator wrapped)
    {
        isIdle = false;
        yield return StartCoroutine(wrapped);
        isIdle = true;
        iterator--;
    }

    private IEnumerator Wait(float delaySeconds)
    {

        yield return new WaitForSeconds(delaySeconds);
    }

    private IEnumerator HuntTarget()
    {
        Vector3 direction;
        Quaternion lookDirection;
        RaycastHit2D hit;

        bool foundTarget = false;

        agent.Warp(transform.position);
        //agent.updatePosition = true;
        while (target != null && !foundTarget) //fill in line of sight condition later
        {
            agent.SetDestination(target.position);
            agent.updatePosition = (character == null || character.GetControl());
            

            if (agent.hasPath)
            {
                //Slowly turn towards where the agent wants to go.
                direction = agent.velocity;
                direction.z = 0f;
                lookDirection = Quaternion.LookRotation(Vector3.forward, direction.normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDirection, maxRotation * Time.deltaTime);


                //Scale agent movement speed with how aligned its movement and the object's heading are.
                //Creates a nice effect where the agent will only really move forward or mostly forward, but spins to align itself when it gets off the path
                agent.speed = Vector2.Dot(direction.normalized, transform.up) * maxSpeed;
                if (agent.speed <= 0f)
                {
                    //Agent wigs out if speed is ever 0 or less so just prevent it from happening
                    agent.speed = 0.01f;
                }
            }

            hit = Physics2D.Raycast(transform.position, transform.up, 20f, LayerMask.GetMask("Player"));
            if (hit.collider != null)
            {
                foundTarget = !agent.Raycast((transform.position + (hit.distance * transform.up)), out _);
                //if (foundTarget) { Debug.Log("Found Target!"); }
            }
            yield return null;
        }

        //agent.ResetPath();
        agent.updatePosition = false;
    }

    /*private IEnumerator AcquireTarget(float delaySeconds)
    {
        float t = 0f;
        Vector2 moveDirection = Vector2.zero;
        while (t <= delaySeconds)
        {
            t += Time.deltaTime;
            moveDirection.x = target.transform.position.x - transform.position.x;
            moveDirection.y = target.transform.position.y - transform.position.y;
            moveDirection.Normalize();
            if (moveDirection != Vector2.zero)
            {
                //transform.rotation = Quaternion.LookRotation(new Vector3 (moveDirection.x, moveDirection.y, 0).normalized);
                transform.rotation = Quaternion.LookRotation(Vector3.back, moveDirection);
            }
            yield return null;
        }
    }

    private IEnumerator RandomDash(float speed, float seconds)
    {
        Vector2 dir = UnityEngine.Random.insideUnitCircle.normalized;
        float t = 0f;
        transform.rotation = Quaternion.LookRotation(Vector3.back, dir);
        while (t <= seconds)
        {
            t += Time.deltaTime;
            if (character.GetControl())
            {
                rb.velocity = dir * speed * dashCurve.Evaluate(t / seconds);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
            yield return null;
        }
        rb.velocity = Vector3.zero;
    }

    private IEnumerator RatScurry()
    {
        float time = UnityEngine.Random.Range(0.25f, 0.75f);

        float distanceLeft = Physics2D.Raycast(transform.position, -transform.right, dashSpeed*time, LayerMask.GetMask("Wall")).distance;
        float distanceRight = Physics2D.Raycast(transform.position, transform.right, dashSpeed*time, LayerMask.GetMask("Wall")).distance;

        if (distanceLeft > distanceRight)
        {
            transform.Rotate(0f, 0f, -90f);
        }
        else if (distanceLeft < distanceRight)
        {
            transform.Rotate(0f, 0f, 90f);
        }
        else
        {
            //pick left or right at random. Generates -1 or 1.
            transform.Rotate(0f, 0f, (UnityEngine.Random.Range(0, 2) * 2 - 1) * 90f);
        }

        GetComponentInChildren<Animator>().SetBool("Run", true);

        float t = 0;
        bool switchedOnce = false;
        
        while(t <= time)
        {
            t += Time.deltaTime;
            if (character.GetControl())
            {
                //divert course to hit player if they're within range
                RaycastHit2D leftRay = Physics2D.Raycast(transform.position, -transform.right, dashSpeed * time * (1 - t / time), LayerMask.GetMask("Player"));
                RaycastHit2D rightRay = Physics2D.Raycast(transform.position, transform.right, dashSpeed * time * (1 - t / time), LayerMask.GetMask("Player"));

                if (!switchedOnce)
                {
                    if (leftRay.collider != null && leftRay.collider.CompareTag("Player"))
                    {
                        transform.Rotate(0f, 0f, 90f);
                        switchedOnce = true;
                    }
                    else if (rightRay.collider != null && rightRay.collider.CompareTag("Player"))
                    {
                        transform.Rotate(0f, 0f, -90f);
                        switchedOnce = true;
                    }
                }

                //maintain velocity as long as in control
                rb.velocity = transform.up * dashSpeed;
            }
            yield return null;
        }

        rb.velocity = Vector2.zero;
        GetComponentInChildren<Animator>().SetBool("Run", false);
    }*/
}
