using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideOnce : MonoBehaviour
{
    private Collider2D thisCollider;
    private List<Collider2D> collidersHit = new List<Collider2D>();

    private void Awake()
    {
        thisCollider = GetComponent<Collider2D>();   
    }

    private void OnDisable()
    {
        Clear();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ignore(Collider2D collider)
    {
        collidersHit.Add(collider);
        Physics2D.IgnoreCollision(thisCollider, collider, true);
    }

    public void Clear()
    {
        foreach (Collider2D collider in collidersHit)
        {
            if (collider != null && collider.enabled)
            { Physics2D.IgnoreCollision(thisCollider, collider, false); }
        }
        collidersHit.Clear();
    }
}
