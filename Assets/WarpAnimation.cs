using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.UIElements;

public class WarpAnimation : MonoBehaviour
{
    private Vector3 position;
    public Vector3 scaleChange;
    private float animationTimer;

    void Awake()
    {
        //position = Vector3.zero;
        scaleChange = new Vector3(2.5f, 2.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = position;
        if (animationTimer > 1.5)
        {
            transform.localScale += (scaleChange * 2 * Time.deltaTime);
        }
        else if (animationTimer > 0.5) { }
        else if (animationTimer > 0)
        {
            transform.localScale -= (scaleChange * 2 * Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector3.zero;
            animationTimer = 0;
            gameObject.SetActive(false);
            return;
        }
        animationTimer -= Time.deltaTime;
    }

    public void StartAnimation(Vector3 edgePosition)
    {
        if (animationTimer == 0)
        {
            position = edgePosition;
            animationTimer = 2;
        }
    }
}
