using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnemy : MonoBehaviour
{
    Vector3 startPosition;

    void Awake()
    {
        startPosition = transform.position;
    }

    void OnEnable()
    {
        transform.position = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
