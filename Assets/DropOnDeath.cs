using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnDeath : MonoBehaviour
{
    public HitboxPickup drop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        GetComponent<Health>().OnDeath += Drop;
    }

    void OnDisable()
    {
        GetComponent<Health>().OnDeath -= Drop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop()
    {
        Instantiate(drop, transform.position, Quaternion.identity, transform.parent);
    }
}
