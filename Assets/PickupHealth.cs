using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : HitboxPickup
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void PickupEffect(Collider2D collector, int amount)
    {
        collector.GetComponent<Health>().GainHealth(amount);
    }
}
