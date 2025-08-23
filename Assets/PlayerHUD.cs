using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    public Character player;
    public GameObject halfheart;
    public Camera cam;

    public Color emptyColor;
    public Color fullColor;

    private GameObject[] halfhearts;

    // Start is called before the first frame update
    void Start()
    {
        halfhearts = new GameObject[player.GetComponent<Health>().maxHealth];
        //float heartWidth = halfheart.GetComponent<SpriteRenderer>().bounds.size.x;
        //Quaternion currentRotation = Quaternion.identity;

        //Vector2 currentPosition = new Vector2(GetComponent<RectTransform>().offsetMin.x, GetComponent<RectTransform>().offsetMax.y);
        Vector2 currentPosition = cam.ScreenToWorldPoint(new Vector2(0.0f, cam.pixelHeight));

        //Draw half hearts and store them in an array
        for(int i = 0; i < player.GetComponent<Health>().maxHealth; i++)
        {
            halfhearts[i] = Instantiate(halfheart, currentPosition, Quaternion.identity, gameObject.transform);

            if (i % 2 == 1)
            {
                halfhearts[i].GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                currentPosition.x += 2 * halfhearts[i].GetComponent<SpriteRenderer>().bounds.size.x;
            }
            halfhearts[i].GetComponent<SpriteRenderer>().color = fullColor;
        }
    }

    private void OnEnable()
    {
        player.GetComponent<Health>().OnHealthChange += UpdateHearts;
    }

    private void OnDisable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHearts(int delta)
    {
        for(int i = 0; i < player.GetComponent<Health>().maxHealth; i++)
        {
            if (i < player.GetComponent<Health>().currentHealth)
            {
                halfhearts[i].GetComponent<SpriteRenderer>().color = fullColor;
            }
            else
            {
                halfhearts[i].GetComponent<SpriteRenderer>().color = emptyColor;
            }
        }
    }
}
