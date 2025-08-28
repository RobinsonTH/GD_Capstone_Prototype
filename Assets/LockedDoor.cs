using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class LockedDoor : Door
{
    // Start is called before the first frame update
    void Start()
    {
        locked = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override bool Open()
    {
        if(locked)
        {
            locked = !transform.parent.parent.parent.GetComponent<Dungeon>().UnlockDoor();
        }
        return !locked;
    }
}
