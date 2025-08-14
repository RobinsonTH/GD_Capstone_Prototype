using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRoll : MonoBehaviour
{
    public AnimationCurve lerpCurve;
    public float distance;
    public float animSeconds;
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
        float currentTime = 0f;
        float remappedTime = 0f;
        Vector3 startPosition = transform.position;

        GetComponent<Character>().LoseControl();
        GetComponent<Health>().invincible = true;

        while (currentTime <= animSeconds)
        {
            
            GetComponent<Character>().HoldControl();
            GetComponent<Health>().invincible = true;

            currentTime += Time.deltaTime;
            remappedTime = lerpCurve.Evaluate(currentTime);
            GetComponent<Rigidbody2D>().MovePosition(Vector3.Lerp(startPosition, (startPosition + (transform.up * distance)), remappedTime));

            yield return null;
        }

        GetComponent<Character>().GainControl();
        GetComponent<Health>().invincible = false;

    }
}
