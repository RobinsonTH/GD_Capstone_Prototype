using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private bool inControl = true;
    // Start is called before the first frame update
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetControl()
    {
        return inControl;
    }

    public void SetControl(bool control)
    {
        inControl = control;
    }
}
