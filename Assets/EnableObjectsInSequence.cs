using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectsInSequence : MonoBehaviour
{
    [SerializeField] List<GameObject> objects;
    [SerializeField] float delay;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerOrigin"))
        {
            StartCoroutine(Sequence());
        }
    }

    private IEnumerator Sequence()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(delay);
        }
    }
}
