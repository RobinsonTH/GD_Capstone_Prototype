using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectsInSequence : MonoBehaviour
{
    [SerializeField] List<GameObject> objects;
    [SerializeField] float delay;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
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
            if (audioSource != null) { audioSource.Play(); }
            yield return new WaitForSeconds(delay);
        }
    }

    public void DisableAll()
    {
        foreach(GameObject obj in objects)
        {
            obj.SetActive(false);
        }
    }
}
