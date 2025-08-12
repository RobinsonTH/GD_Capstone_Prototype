using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Equipment equipped;

    private bool inControl = true;
    // Start is called before the first frame update
    void Start()
    {
        if (equipped != null) { equipped.OnEquip(); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        
    }

    public bool GetControl()
    {
        return inControl;
    }

    public void LoseControl()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        inControl = false;
    }

    public void GainControl()
    {
        inControl = true;
    }

    public void Equip(Equipment equip)
    {
        if (equipped != null) { equipped.OnEquip(); }
        equipped = equip;
        equipped.OnEquip();
    }

    public void FireEquipment()
    {
        if (equipped != null) { StartCoroutine(equipped.Fire(this)); }
    }
}
