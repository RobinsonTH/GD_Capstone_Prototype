using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantPortal : MonoBehaviour
{
    bool animationFinished;
    // Start is called before the first frame update
    void Start()
    {
        animationFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerOrigin"))
        {
            StartCoroutine(GrantWarp(collision.transform.parent.gameObject));
        }
    }

    private IEnumerator GrantWarp(GameObject player)
    {
        Character p = player.GetComponent<Character>();
        p.LoseControl();

        player.transform.eulerAngles = new Vector3(0, 0, 180);

        GetComponent<Animator>().SetTrigger("TriggerGetPortal");
        yield return new WaitUntil(() => animationFinished);

        
        player.AddComponent<PlayerWarp>();
        p.GainControl();
        Destroy(gameObject);
    }

    public void FadeRunes()
    {
        transform.parent.GetComponent<Animator>().SetTrigger("PortalAbsorbed");
    }

    public void EndAnimation()
    {
        GetComponent<SpriteRenderer>().enabled = false; 
        animationFinished = true;
    }
}
