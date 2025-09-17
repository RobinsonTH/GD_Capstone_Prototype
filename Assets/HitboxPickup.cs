using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class HitboxPickup : MonoBehaviour
{
    //public string targetTag;
    public int amount;
    private AudioClip clip;
    //public Pickup pickup;

    private void Awake()
    {
        clip = Resources.Load<AudioClip>("Sounds/RPG_Essentials_Free/10_UI_Menu_SFX/051_use_item_01");
    }
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
            AudioSource.PlayClipAtPoint(clip, collision.transform.position);
            PickupEffect(collision, amount);
            Destroy(gameObject);
        }
    }

    protected abstract void PickupEffect(Collider2D collector, int amount);
}
