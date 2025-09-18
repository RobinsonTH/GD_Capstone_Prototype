using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.U2D;

public class LockedDoor : Door
{
    // Start is called before the first frame update
    void Awake()
    {
        locked = true;
        sprite = GetComponent<SpriteRenderer>();
        clip = Resources.Load<AudioClip>("Sounds/RPG_Essentials_Free/10_UI_Menu_SFX/071_Unequip_01");
        unlockedColor = sprite.color;
        lockedColor = unlockedColor;
        lockedColor.r *= 0.5f;
        lockedColor.b *= 0.5f;
        lockedColor.g *= 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override bool Open()
    {
        if(locked && transform.parent.parent.parent.GetComponent<Dungeon>().UnlockDoor())
        {
            Unlock();
        }
        return !locked;
    }
}
