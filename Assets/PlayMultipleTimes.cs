using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMultipleTimes : MonoBehaviour
{
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(int iterator)
    {
        StartCoroutine(PlaySound(iterator));
    }

    private IEnumerator PlaySound(int iterator)
    {
        for (int i = 0; i < iterator; i++)
        {
            //Debug.Log("Playing");
            audioSource.Play();
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => !audioSource.isPlaying);
        }
    }
}
