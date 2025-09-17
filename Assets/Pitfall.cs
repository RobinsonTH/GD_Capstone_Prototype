using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pitfall : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Room targetRoom;
    [SerializeField] private float duration = 1.5f;
    [SerializeField] private int fallDamage = 2;

    private AudioClip fallClip;
    private AudioClip landClip;

    private void Awake()
    {
        fallClip = Resources.Load<AudioClip>("Sounds/RPG_Essentials_Free/8_Buffs_Heals_SFX/44_Sleep_01");
        landClip = Resources.Load<AudioClip>("Sounds/RPG_Essentials_Free/12_Player_Movement_SFX/45_Landing_01");
    }

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
        collision.GetComponent<Character>().LoseControlNoStop();
        collision.GetComponent<Health>().GainInvincibility();
        float t = 0;
        float normalizedTime = 0;
        Vector3 originalScale = collision.localScale;
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null) { rb.velocity.Normalize(); }
        while (t <= duration)
        {
            t += Time.deltaTime;
            normalizedTime = 1 - (t / duration);
            collision.localScale = new Vector3(normalizedTime, normalizedTime, 1);

            if (rb != null) { rb.velocity = rb.velocity.normalized * normalizedTime; }
            yield return null;
        }
        collision.localScale = originalScale;
    }

    private IEnumerator FallToDeath(Transform enemy)
    {
        yield return Fall(enemy);
        enemy.GetComponent<Health>().Die();
    }

    private IEnumerator FallDown(Transform player)
    {
        AudioSource.PlayClipAtPoint(fallClip, player.position);
        yield return Fall(player);
        player.parent.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        player.position = targetRoom.transform.position;
        player.GetComponent<Health>().LoseInvincibility();
        player.GetComponent<Health>().TakeDamage(fallDamage);
        player.GetComponent<Character>().GainControl();
        yield return new WaitForSeconds(1.0f);
        AudioSource.PlayClipAtPoint(landClip, player.position, 5.0f);
    }
}
