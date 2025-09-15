using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRoll : MonoBehaviour
{
    public AnimationCurve lerpCurve;
    //public float distance;
    [SerializeField] private float dashSpeed;
    public float animSeconds;

    Character character;
    Health health;
    Rigidbody2D rb;

    private void Awake()
    {
        character = GetComponent<Character>();
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Roll()
    {
        yield return null;
        float lastTime = 0f;
        float currentTime = 0f;

        float speedLoss = 0f;
        float newSpeed = dashSpeed;
        //float remappedTime = 0f;
        //Vector3 startPosition = transform.position;

        character.LoseControl();
        health.GainInvincibility();

        rb.velocity = transform.up * dashSpeed;

        /*while (currentTime <= animSeconds)
        {
            
            character.HoldControl();
            health.invincible = true;

            currentTime += Time.deltaTime;
            remappedTime = lerpCurve.Evaluate(currentTime/animSeconds);
            rb.MovePosition(Vector3.Lerp(startPosition, (startPosition + (transform.up * distance)), remappedTime));

            yield return null;
        }*/

        while (currentTime <= animSeconds)
        {
            //character.HoldControl();
            //health.invincible = true;

            lastTime = currentTime;
            currentTime += Time.deltaTime;
            speedLoss = dashSpeed * (lerpCurve.Evaluate(currentTime/animSeconds) - lerpCurve.Evaluate(lastTime/animSeconds));
            newSpeed = rb.velocity.magnitude + speedLoss;
            if (newSpeed < 0f)
            {
                newSpeed = 0f;
            }
            rb.velocity = rb.velocity.normalized * newSpeed;

            yield return null;

        }

        character.GainControl();
        health.LoseInvincibility();

    }
}
