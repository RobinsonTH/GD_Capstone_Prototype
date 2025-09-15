using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackTowardsPoint : MonoBehaviour
{
    [SerializeField] Transform targetPoint;
    [SerializeField] List<string> targetTags = new List<string>();

    [SerializeField] float time;
    [SerializeField] float magnitude;
    // Start is called before the first frame update

    private void OnTriggerStay2D(Collider2D collision)
    {
        Knockback(collision);
    }

    private void Knockback(Collider2D collision)
    {
        foreach (string tag in targetTags)
        {
            if (collision.CompareTag(tag))
            {
                //Knockback knockback = collision.GetComponentInParent<Knockback>();
                Vector3 knockback = (targetPoint.position - collision.transform.position).normalized * magnitude;
                collision.GetComponentInParent<Knockback>()?.TakeManualKnockback(time, knockback);
                return;
            }
        }
    }
}
