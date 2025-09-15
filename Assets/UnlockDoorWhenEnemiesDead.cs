using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoorWhenEnemiesDead : MonoBehaviour
{
    [SerializeField] List<Door> doors;

    private Character[] enemies;
    private int enemyCount;
    private bool fullyLocked = false;


    private void Awake()
    {
        //enemies = GetComponent<Room>().GetCharacterList();
    }

    private void OnEnable()
    {
        
        enemies = GetComponentsInChildren<Character>(); //GetComponent<Room>().GetCharacterList();

        //Debug.Log("Discovered " +  enemies.Length + " Enemies");
        foreach (Character c in enemies)
        {
            Health h = c.GetComponentInParent<Health>();
            if(h != null)
            {
                //Debug.Log("Subscribed to an OnDeath event!");
                h.OnDeath += Check;
                enemyCount++;
            }
        }
        //Debug.Log("Enemy Count: " + enemyCount);

    }

    private void OnDisable()
    {
        foreach (Character c in enemies)
        {
            Health h = c.GetComponentInParent<Health>();
            if (h != null)
            {
                h.OnDeath -= Check;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (fullyLocked == false && collision.CompareTag("PlayerOrigin"))
        {
            int lockedDoors = 0;
            foreach (Door door in doors)
            {
                if(door.GetComponent<SpriteRenderer>().enabled)
                {
                    
                    door.Lock();
                    lockedDoors++;
                }
                
            }
            if(lockedDoors == doors.Count)
            {
                fullyLocked = true;
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerOrigin"))
        {
            fullyLocked = false;
            foreach(Door door in doors)
            {
                door.Unlock();
            }
        }
    }

    void Check()
    {
        enemyCount--;
        if(enemyCount <= 0)
        {
            foreach (Door door in doors)
            {
                door.Unlock();
            }
            Destroy(this);
        }
    }
}
