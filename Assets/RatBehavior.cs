using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RatBehavior : MonoBehaviour
{

    [SerializeField] private Character character;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject target;
    [SerializeField] private AnimationCurve dashCurve;
    [SerializeField] private float dashSpeed;

    private bool isIdle = true;
    private IEnumerator coroutine;
    private enum State
    {
        Move,
        AcquireTarget,
        Fire,
        RatScurry,
        RandomDash,
        Wait
    }

    [SerializeField] private State startState;
    private State state = State.AcquireTarget;
    private int iterator = 1;

    // Start is called before the first frame update
    void Start()
    {
        
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
                case State.Move:
                    break;
                case State.AcquireTarget:
                    coroutine = AcquireTarget(0.1f);
                    state = State.Fire;
                    iterator = 0;
                    break;
                case State.Fire:
                    coroutine = character.equipped.Fire(character);
                    if (iterator >= 1) 
                    { 
                        state = State.RandomDash;
                        iterator = 0;
                    }
                    break;
                case State.RatScurry:
                    coroutine = RatScurry();
                    if(iterator >= 3)
                    {
                        state = State.Wait;
                    }
                    break;
                case State.RandomDash:
                    coroutine = RandomDash(dashSpeed, 0.5f);
                    if (iterator >= 3)
                    {
                        state = State.AcquireTarget;
                        iterator = 0;
                    }
                    break;
                case State.Wait:
                    coroutine = Wait(1.5f);
                    state = State.RatScurry;
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
        iterator++;
    }

    private IEnumerator Wait(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
    }

    private IEnumerator AcquireTarget(float delaySeconds)
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
        float distanceLeft = Physics2D.Raycast(transform.position, -transform.right, 4f, LayerMask.GetMask("Wall")).distance;
        float distanceRight = Physics2D.Raycast(transform.position, transform.right, 4f, LayerMask.GetMask("Wall")).distance;

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
            transform.Rotate(0f, 0f, UnityEngine.Random.Range(-1, 2) * 90f);
        }



            float t = 0;
        while(t <= 0.75f)
        {
            t += Time.deltaTime;
            if (character.GetControl())
            {
                rb.velocity = transform.up * dashSpeed;
            }
            yield return null;
        }

        rb.velocity = Vector2.zero;

    }
}
