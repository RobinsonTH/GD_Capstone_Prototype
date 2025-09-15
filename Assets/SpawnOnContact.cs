using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnContact : MonoBehaviour
{
    [SerializeField] GameObject spawn;
    [SerializeField] List<string> targetTags;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //foreach (string tag in targetTags)
        //{
            //if(collision.collider.CompareTag(tag))
            //{
                //Debug.Log(collision.collider.transform.name);
                //Debug.Log("Collision!");
                Instantiate(spawn, collision.GetContact(0).point, Quaternion.identity);
                return;
            //}
        //}

    }
}
