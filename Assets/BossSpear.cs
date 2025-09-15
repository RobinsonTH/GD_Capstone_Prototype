using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpear : MonoBehaviour
{
    [SerializeField] private GameObject spear;
    //private Quaternion targetAim;
    //private bool isAiming;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThrowSpear()
    {
        Instantiate(spear, transform.position, transform.rotation);
    }

    /*void LateUpdate()
    {
        if(isAiming)
        {
            transform.rotation = targetAim;
            isAiming = false;
        }
    }*/

    /*public void AimAtTarget(Quaternion target)
    {
        isAiming = true;
        targetAim = target;
    }*/
}
