using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermittentSpawner : MonoBehaviour
{
    [SerializeField] GameObject spawn;
    [SerializeField] float offSeconds;
    [SerializeField] float delaySeconds;
    [SerializeField] int spawnCount;
    [SerializeField] float spawnLifetime;

    private float secondsPassed;
    GameObject[] objects;

    // Start is called before the first frame update
    void Start()
    {
        objects = new GameObject[spawnCount];
    }

    private void OnEnable()
    {
        secondsPassed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        secondsPassed += Time.deltaTime;
        if(secondsPassed >= offSeconds)
        {
            StartCoroutine(SpawnObjects());
            secondsPassed = -(delaySeconds * spawnCount);
        }
    }

    private IEnumerator SpawnObjects()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            objects[i] = Instantiate(spawn, gameObject.transform);
            Destroy(objects[i], spawnLifetime);
            yield return new WaitForSeconds(delaySeconds);
        }
    }
}
