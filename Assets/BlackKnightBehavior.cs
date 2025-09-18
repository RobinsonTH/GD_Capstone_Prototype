using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class BlackKnightBehavior : MonoBehaviour
{

    private Character character;
    private Rigidbody2D rb;
    [SerializeField] private DetectionRange northRange;
    [SerializeField] private DetectionRange westRange;
    [SerializeField] private DetectionRange eastRange;
    [SerializeField] private DetectionRange southRange;
    //private NavMeshAgent agent;
    private Transform target;
    //private Transform target;
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;

    [SerializeField] private Collider2D spear;
    private BossSpear spearHand;
    //[SerializeField] private GameObject thrownSpear;
    [SerializeField] GameObject shockwave;

    private bool isIdle = true;
    private IEnumerator coroutine;
    //private Coroutine currentRoutine = null;
    private enum State
    {
        /*Move,
        AcquireTarget,
        Fire,
        RatScurry,
        RandomDash,
        TurnRadius,*/
        //HuntTarget,
        MonitorLocation,
        JumpTurn,
        SwingSpear,
        FanSpear,
        ThrowSpear,
        //Fire,
        Wait
    }

    private enum Direction
    {
        North,
        West,
        East,
        South,
        Null
    }

    [SerializeField] private State startState;
    [SerializeField] private int firstIterator;
    //[SerializeField] private float maxSpeed;
    //[SerializeField] private float maxRotation;
    private Direction orientation = Direction.Null;
    private Direction targetOrientation = Direction.Null;

    private State state;
    private int iterator;
    //private Vector2 startPosition;
    //private Quaternion startRotation;

    private void Awake()
    {
        target = null;
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
        spearHand = spear.transform.parent.GetComponent<BossSpear>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //agent.updateRotation = false;
        //agent.updateUpAxis = false;
        //agent.updatePosition = false;
    }

    private void OnEnable()
    {
        state = startState;
        iterator = firstIterator;
        isIdle = true;

        //spriteHandler.enabled = false;

        //subscribe to detectedtarget
        northRange.OnDetect += SpotTargetNorth;
        westRange.OnDetect += SpotTargetWest;
        eastRange.OnDetect += SpotTargetEast;
        southRange.OnDetect += SpotTargetSouth;
    }

    private void OnDisable()
    {
        northRange.OnDetect -= SpotTargetNorth;
        westRange.OnDetect -= SpotTargetWest;
        eastRange.OnDetect -= SpotTargetEast;
        southRange.OnDetect -= SpotTargetSouth;

        //Reset all values
        target = null;
        animator.SetBool("IsJumping", false);
        orientation = Direction.Null;
        targetOrientation = Direction.Null;
        sprite.sprite = LoadSprite("black_knight_asleep");
}

    // Update is called once per frame
    void Update()
    {
        if (isIdle && character.GetControl())
        {
            if(targetOrientation != Direction.Null && orientation != targetOrientation)
            {
                coroutine = JumpTurn();
                //iterator++;
            }
            else
            {
                switch (state)
                {
                    /*case State.Move:
                        break;
                    case State.AcquireTarget:
                        coroutine = AcquireTarget(0.1f);
                        state = State.Fire;
                        iterator = 0;
                        break;
                    case State.HuntTarget:
                        coroutine = HuntTarget();
                        if (iterator <= 0)
                        {
                            state = State.Fire;
                            iterator = 1;
                        }
                        break;*/
                    case State.MonitorLocation:
                        coroutine = MonitorLocation();
                        state = State.ThrowSpear;
                        iterator = Math.Max(2, 8 * (health.currentHealth / health.maxHealth));
                        break;
                    /*case State.Fire:
                        coroutine = character.equipped.Fire(character);
                        state = State.Wait;
                        iterator = 1;
                        break;*/
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
                    case State.SwingSpear:
                        coroutine = SwingSpear();
                        state = State.Wait;
                        iterator = 1;
                        break;
                    case State.FanSpear:
                        coroutine = FanSpear();
                        state = State.ThrowSpear;
                        iterator = Math.Max(2, (int)Math.Ceiling(8 * (float)health.currentHealth / health.maxHealth));
                        break;
                    case State.ThrowSpear:
                        coroutine = SpearThrow();
                        if (iterator <= 0)
                        {
                            state = State.SwingSpear;
                            iterator = 1;
                        }
                        break;
                    case State.Wait:
                        coroutine = Wait(1f);
                        state = State.FanSpear;
                        iterator = 1;
                        break;
                    default:
                        break;
                }
            }
                
            //Debug.Log("State Machine reached a decision. Iterator = " + iterator);
            //Debug.Log("Next State is " + state.ToString());
            StartCoroutine(WrapperCoroutine(coroutine));
        }
    }

    private IEnumerator WrapperCoroutine(IEnumerator wrapped)
    {
        isIdle = false;
        yield return StartCoroutine(wrapped);
        isIdle = true;
        iterator--;
        //Debug.Log("Wrapper Complete");
    }

    private IEnumerator Wait(float delaySeconds)
    {

        yield return new WaitForSeconds(delaySeconds);
    }

    /*private IEnumerator HuntTarget()
    {
        //Debug.Log("Hunting");
        Vector3 direction;
        Quaternion lookDirection;
        RaycastHit2D hit;

        bool foundTarget = false;

        agent.Warp(transform.position);
        //agent.updatePosition = true;
        while (target != null && !foundTarget) //fill in line of sight condition later
        {
            agent.SetDestination(target.position);
            agent.updatePosition = character.GetControl();
            

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
                animator.SetFloat("WalkSpeed", agent.speed);
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
        animator.SetFloat("WalkSpeed", 0f);
    }*/

    private IEnumerator MonitorLocation()
    {
        health.GainInvincibility();
        yield return new WaitUntil(() => (target != null));
        
        sprite.sprite = LoadSprite("black_knight_south");

        yield return new WaitForSeconds(1f);
        health.LoseInvincibility();
    }
    void SpotTarget(GameObject newTarget)
    {
        if (target == null) { target = newTarget.transform; }
    }
    public void SpotTargetNorth(GameObject newTarget)
    {
        SpotTarget(newTarget);
        targetOrientation = Direction.North;
        northRange.LoseTarget();

    }
    public void SpotTargetWest(GameObject newTarget)
    {
        SpotTarget(newTarget);
        targetOrientation = Direction.West;
        westRange.LoseTarget();
    }
    public void SpotTargetEast(GameObject newTarget)
    {
        SpotTarget(newTarget);
        targetOrientation = Direction.East;
        eastRange.LoseTarget();
    }
    public void SpotTargetSouth(GameObject newTarget)
    {
        SpotTarget(newTarget);
        targetOrientation = Direction.South;
        southRange.LoseTarget();
    }

    private IEnumerator JumpTurn()
    {
        Collider2D[] parts = GetComponentsInChildren<Collider2D>();
        yield return new WaitUntil(() => !animator.GetBool("IsJumping"));
        animator.SetBool("IsJumping", true);
        for(int i = 0; i < parts.Length; i++)
        {
            parts[i].enabled = false;
        }
        yield return new WaitUntil(() => !animator.GetBool("IsJumping"));
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].enabled = true;
        }
        yield return new WaitForSeconds(1.5f);
    }

    public void Turn()
    {
        Vector3 newOrientation = transform.eulerAngles;
        switch(targetOrientation)
        {
            case Direction.North:
                newOrientation.z = 0f;
                break;
            case Direction.West:
                newOrientation.z = 90f;
                break;
            case Direction.East:
                newOrientation.z = 270f;
                break;
            case Direction.South:
                newOrientation.z = 180f;
                break;
            default:
                break;
        }
        transform.eulerAngles = newOrientation;
        orientation = targetOrientation;
    }

    public void Land()
    {
        animator.SetBool("IsJumping", false);
        //Instantiate shockwave
        shockwave.SetActive(true);
    }

    public IEnumerator SwingSpear()
    {
        spearHand.transform.localRotation = Quaternion.identity;
        animator.SetTrigger("SpearSwing");
        yield return new WaitUntil(() => spear.gameObject.activeSelf);
        yield return new WaitUntil(() => !spear.gameObject.activeSelf);
        //Debug.Log("Done Swinging");
    }


    public void AimSpear()
    {
        Vector3 targetDirection = target.transform.position - transform.position;
        spearHand.transform.rotation = Quaternion.LookRotation(Vector3.forward, targetDirection);

        Vector3 clampedLocal = spearHand.transform.localEulerAngles;
        if (clampedLocal.z > 40f && clampedLocal.z <= 180f)
        {
            clampedLocal = new Vector3(clampedLocal.x, clampedLocal.y, 40f);
        }
        else if (clampedLocal.z > 180f && clampedLocal.z < 320f)
        {
            clampedLocal = new Vector3(clampedLocal.x, clampedLocal.y, 320f);
        }
        spearHand.transform.localEulerAngles = clampedLocal;
        //Debug.Log("Aimed Spear");
    }

    private IEnumerator FanSpear()
    {
        Vector3 throwAngle = transform.localEulerAngles;
        throwAngle.z += 120f;

        for(int i = 0; i < 7; i++)
        {
            throwAngle.z -= 30f;
            spearHand.transform.rotation = Quaternion.Euler(throwAngle);
            spearHand.ThrowSpear();

            yield return new WaitForSeconds(0.33f);
        }
    }

    private IEnumerator SpearThrow()
    {
        AimSpear();
        spearHand.ThrowSpear();
        yield return new WaitForSeconds(1f);
    }

    /*public void LoseTarget()
    {
        target = null;
        //Subscribe to detectedtarget
        range.OnDetect += SpotTarget;
        range.OnLostTarget -= LoseTarget;
    }*/

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
    Sprite LoadSprite(string sprite)
    {
        Sprite[] sheet = Resources.LoadAll<Sprite>("Sprites/black_knight_sheet");

        foreach (Sprite s in sheet)
        {
            if (s.name == sprite)
            {
                return s;
            }
        }
        return null;

    }

    public void ClearHits()
    {
        spear.GetComponent<CollideOnce>().Clear();
    }
}
