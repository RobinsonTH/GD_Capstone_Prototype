using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickInWall : MonoBehaviour
{
    [SerializeField] float duration;

    private Coroutine sticking = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void OnDestroy()
    {
        if (sticking != null) { transform.parent.GetComponent<Character>().GainControl(); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall"))
        {
            sticking = StartCoroutine(Stick());
        }
    }

    private IEnumerator Stick()
    {
        transform.parent.GetComponent<Character>().LoseControl();
        yield return new WaitForSeconds(duration);
        transform.parent.GetComponent<Character>().GainControl();
        sticking = null;
        Destroy(gameObject);
    }
}
