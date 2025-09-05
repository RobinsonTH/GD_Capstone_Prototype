using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonEnemy : MonoBehaviour
{
    Vector3 startPosition;
    Quaternion startRotation;

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void OnEnable()
    {
        //Debug.Log("teleported back to starting position");
        if(TryGetComponent(out NavMeshAgent agent))
        {
            //Debug.Log("Warping");
            agent.Warp(startPosition);
        }
        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
