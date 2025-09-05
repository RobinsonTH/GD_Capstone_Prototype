using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualKnockback : MonoBehaviour
{
    public float time;
    [SerializeField] Vector2 knockbackVector;
    //public float magnitude;
    //public string targetTag;
    [SerializeField] List<string> targetTags;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach (string tag in targetTags)
        {
            if (collision.CompareTag(tag))
            {
                //Knockback knockback = collision.GetComponentInParent<Knockback>();
                collision.GetComponentInParent<Knockback>()?.TakeManualKnockback(time, knockbackVector);
                break;
                //Debug.Log("Knocking Back");
            }
        }



        //Debug.Log(gameObject.GetComponent<Collider2D>().ToString());
        //collision.GetComponentInParent<Knockback>()?.TakeKnockback(time, magnitude, GetComponent<Collider2D>());
    }
}
