using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnDeath : MonoBehaviour
{
    [SerializeField] int iterator = 1;
    private Health health;
    [SerializeField] private PlayMultipleTimes audioSource;
    

    private void Awake()
    {
        health = GetComponent<Health>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        health.OnDeath += Play;
    }

    private void OnDisable()
    {
        health.OnDeath -= Play;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Play()
    {
        audioSource.Play(iterator);
        health.OnDeath -= Play;
    }

    
}
