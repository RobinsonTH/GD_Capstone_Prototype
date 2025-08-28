using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BehaviorMachine : MonoBehaviour
{

    [SerializeField] private Character character;
    [SerializeField] private GameObject target;
    private bool isIdle = true;
    private IEnumerator coroutine;
    private enum State
    {
        Move,
        AcquireTarget,
        Fire,
        Wait
    }

    [SerializeField] private State state;
    private int iterator = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        state = State.AcquireTarget;
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
                    coroutine = AcquireTarget(2.0f);
                    state = State.Fire;
                    iterator = 0;
                    break;
                case State.Fire:
                    coroutine = character.equipped.Fire(character);
                    if (iterator >= 3) 
                    { 
                        state = State.AcquireTarget;
                        iterator = 0;
                    }
                    break;
                case State.Wait:
                    coroutine = Wait(2.0f);
                    state = State.Fire;
                    break;
                default:
                    break;
            }
            Debug.Log("State Machine reached a decision. Iterator = " + iterator);
            Debug.Log("Current State is " + state.ToString());
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
        float t = 0;
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
}
