using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staircase : MonoBehaviour
{
    [SerializeField] private Transform point;
    private AudioClip sound;

    private void Awake()
    {
        sound = Resources.Load<AudioClip>("Sounds/fx/enter");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerOrigin"))
        {
            
            collision.transform.parent.position = point.position;
            AudioSource.PlayClipAtPoint(sound, point.position, 20.0f);
        }
    }
}
