using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandOverTime : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float targetSize;

    private float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        time = 0f;
    }

    private void OnDisable()
    {
        transform.localScale = new Vector3(0f, 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        float newSize = targetSize * (time / duration);
        transform.localScale = new Vector3(newSize, newSize, 1f);
        if(time >= duration)
        {
            gameObject.SetActive(false);
        }
    }
}
