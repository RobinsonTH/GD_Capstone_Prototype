using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class HitboxPickup : MonoBehaviour
{
    //public string targetTag;
    public int amount;
    //public Pickup pickup;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickupEffect(collision, amount);
            Destroy(gameObject);
        }
    }

    protected abstract void PickupEffect(Collider2D collector, int amount);
}
