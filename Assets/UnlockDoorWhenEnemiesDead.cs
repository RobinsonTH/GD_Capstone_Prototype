using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoorWhenEnemiesDead : MonoBehaviour
{
    [SerializeField] Door door;

    private Character[] enemies;
    private int enemyCount;


    private void Awake()
    {
        //enemies = GetComponent<Room>().GetCharacterList();
    }

    private void OnEnable()
    {
        door.Lock();
        enemies = GetComponentsInChildren<Character>(); //GetComponent<Room>().GetCharacterList();

        Debug.Log("Discovered " +  enemies.Length + " Enemies");
        foreach (Character c in enemies)
        {
            if(c.GetComponent<Health>())
            {
                Debug.Log("Subscribed to an OnDeath event!");
                c.GetComponent<Health>().OnDeath += Check;
                enemyCount++;
            }
        }
        Debug.Log("Enemy Count: " + enemyCount);

    }

    private void OnDisable()
    {
        foreach (Character c in enemies)
        {
            if (c.GetComponent<Health>())
            {
                c.GetComponent<Health>().OnDeath -= Check;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Check()
    {
        enemyCount--;
        if(enemyCount <= 0)
        {
            door.Unlock();
            Debug.Log("Unlocking Door");
        }
        Debug.Log("Enemy died! New Count: " + enemyCount);
    }
}
