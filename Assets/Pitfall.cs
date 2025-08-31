using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pitfall : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Room targetRoom;
    [SerializeField] private float duration;
    [SerializeField] private int fallDamage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EnemyOrigin"))
        {
            StartCoroutine(FallToDeath(collision.transform.parent));
        }
        else if(collision.CompareTag("PlayerOrigin"))
        {
            StartCoroutine(FallDown(collision.transform.parent));
        }
    }

    private IEnumerator Fall(Transform collision)
    {
        collision.GetComponent<Character>().LoseControl();
        collision.GetComponent<Health>().invincible = true;
        float t = 0;
        float normalizedTime = 0;
        while (t <= duration)
        {
            collision.GetComponent<Character>().HoldControl();
            t += Time.deltaTime;
            normalizedTime = 1 - (t / duration);
            collision.localScale = new Vector3(normalizedTime, normalizedTime, 1);
            yield return null;
        }
        collision.localScale = Vector3.one;
    }

    private IEnumerator FallToDeath(Transform enemy)
    {
        yield return Fall(enemy);
        enemy.GetComponent<Health>().Die();
    }

    private IEnumerator FallDown(Transform player)
    {
        yield return Fall(player);
        player.parent.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        player.position = targetRoom.transform.position;
        player.GetComponent<Health>().invincible = false;
        player.GetComponent<Health>().TakeDamage(fallDamage);
        player.GetComponent<Character>().GainControl();
    }
}
