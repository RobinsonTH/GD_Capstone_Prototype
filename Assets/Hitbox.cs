using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float damage;
    public float recoilTime;
    public float recoilMagnitude;
    public string targetTag;

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
        if (collision.CompareTag(targetTag))
        {
           collision.GetComponent<TestEnemyAI>().TakeDamage(damage, recoilTime, recoilMagnitude, this.GetComponent<Collider2D>());
        }
    }
}
