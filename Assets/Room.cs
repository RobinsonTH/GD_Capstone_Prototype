using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Character[] characters;
    private bool explored;
    private bool active;

    private void Awake()
    {
        characters = PopulateCharacterList();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        DisableCharacters();
        //Debug.Log("Start+Disable");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerOrigin")
        {
            explored = true;
            active = true;
            collision.transform.parent.SetParent(gameObject.transform, true);
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(transform.parent.GetComponentInParent<Dungeon>().SetCameraToRoom(this));
            EnableCharacters();
            //Debug.Log("Entered+Enabled");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayerOrigin")
        {
            active = false;
            GetComponent<SpriteRenderer>().enabled = true;
            DisableCharacters();
            //Debug.Log("Left+Disabled");
        }

    }

    private Character[] PopulateCharacterList()
    {
        return GetComponentsInChildren<Character>();
    }

    private void DisableCharacters()
    {
        foreach (Character character in characters)
        {
            character.gameObject.SetActive(false);
        }
    }

    private void EnableCharacters()
    {
        foreach (Character character in characters)
        {
            character.gameObject.SetActive(true);
        }
    }

    public Character[] GetCharacterList()
    {
        return characters;
    }

    public bool IsExplored()
    {
        return explored;
    }

    public bool IsActive()
    {
        return active;
    }
}
