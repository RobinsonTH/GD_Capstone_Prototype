using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Only use this one until DropTable solution is fixed

public class DropRandomOnDeath : MonoBehaviour
{
    [SerializeField] List<GameObject> drops;
    private Health health;

    void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.OnDeath += Roll;
    }

    private void OnDisable()
    {
        health.OnDeath -= Roll;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Roll()
    {
        int roll = UnityEngine.Random.Range(0, (drops.Count + 1));
        if (roll == drops.Count) { return; }
        else
        {
            Instantiate(drops[roll], transform.position, Quaternion.identity, transform.parent);
        }
    }
}
