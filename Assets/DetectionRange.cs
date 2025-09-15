using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DetectionRange : MonoBehaviour
{
    [SerializeField] List<string> targetTags;

    public delegate void DetectedTarget(GameObject target);
    public DetectedTarget OnDetect;

    public delegate void LostTarget();
    public LostTarget OnLostTarget;

    private GameObject currentTarget;

    private void Awake()
    {
        currentTarget = null;
        if(targetTags == null) {targetTags = new List<string>();}
    }

    private void Update()
    {
        if(currentTarget != null && !TargetInLineOfSight(currentTarget.transform))
        {
            LoseTarget();
        }
    }

    private void OnDisable()
    {
        if(currentTarget != null)
        {
            LoseTarget();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(OnDetect != null && currentTarget == null)
        {
            foreach (string tag in targetTags)
            {
                if (collision.CompareTag(tag))// && TargetInLineOfSight(collision.transform))
                {
                    AcquireTarget(collision.gameObject);
                    return;
                }
            }
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if(OnLostTarget != null && collision.gameObject == currentTarget)
        {
            LoseTarget();
        }
    }*/

    private void AcquireTarget(GameObject target)
    {
        currentTarget = target;
        if (OnDetect != null) { OnDetect(currentTarget); }
    }

    public void LoseTarget()
    {
        currentTarget = null;
        if (OnLostTarget != null) { OnLostTarget(); }
    }

    private bool TargetInLineOfSight(Transform target)
    {
        Vector2 direction = (target.position - transform.parent.position);

        return (Physics2D.Raycast(transform.parent.position, direction, direction.magnitude, LayerMask.GetMask("Wall")).collider == null);
    }
}
