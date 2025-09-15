using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    Health health;
    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.OnDeath += Permadie;
    }

    private void OnDisable()
    {
        health.OnDeath -= Permadie;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Permadie()
    {
        health.OnDeath -= Permadie;
        Destroy(gameObject);
    }
}
