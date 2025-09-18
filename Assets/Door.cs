using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] protected bool locked = false;
    public Collider2D door;

    protected SpriteRenderer sprite;
    protected Color unlockedColor;
    protected Color lockedColor;

    protected AudioClip clip;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        clip = Resources.Load<AudioClip>("Sounds/RPG_Essentials_Free/10_UI_Menu_SFX/071_Unequip_01");
        unlockedColor = sprite.color;
        lockedColor = unlockedColor;
        lockedColor.r *= 0.5f;
        lockedColor.b *= 0.5f;
        lockedColor.g *= 0.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!locked)
        {
            door.excludeLayers += LayerMask.GetMask("Player");
            sprite.color = unlockedColor;
        }
        else
        {
            sprite.color = lockedColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Open())
            {
                AudioSource.PlayClipAtPoint(clip, transform.position);
                GetComponent<SpriteRenderer>().enabled = false;
                //door.excludeLayers = LayerMask.GetMask("Player");
            }
        }
    }

    public void Lock()
    {
        if(!locked)
        {
            locked = true;
            door.excludeLayers -= LayerMask.GetMask("Player");
            sprite.color = lockedColor;
        }
        
    }

    public void Unlock()
    {
        if(locked)
        {
            locked = false;
            door.excludeLayers += LayerMask.GetMask("Player");
            sprite.color = unlockedColor;
        }
        
    }

    protected virtual bool Open()
    {
        return !locked;
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = true;
            //door.excludeLayers += LayerMask.GetMask("Player");
        }
    }
}
