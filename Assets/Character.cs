using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Equipment equipped;
    private Coroutine firingEquipment;
    private Health health;

    //private bool inControl = true;
    private int lockouts = 0;
    // Start is called before the first frame update

    private void Awake()
    {
        health = GetComponentInParent<Health>();
    }
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
        //inControl = true;
        lockouts = 0;
        if(health != null)
        {
            health.enabled = true;
            health.OnDamage += InterruptEquipment;
        }
        //if (GetComponent<SpriteRenderer>())
        //{
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        //}
    }

    private void OnDisable()
    {
        //inControl = false;
        lockouts = 1;
        if (health != null)
        {
            health.OnDamage -= InterruptEquipment;
            health.enabled = false;
        }
    }

    public bool GetControl()
    {
        return (lockouts == 0);
    }

    public void LoseControl()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        //inControl = false;
        lockouts++;
    }

    public void LoseControlNoStop()
    {
        lockouts++;
    }

    /*public void HoldControl()
    {
        //inControl = false;
    }*/

    public void GainControl()
    {
        //inControl = true;
        lockouts--;
    }

    public void Equip(Equipment equip)
    {
        if (equipped != null) { equipped.OnEquip(); }
        equipped = equip;
        equipped.OnEquip();
    }

    public void FireEquipment()
    {
        if (equipped != null) { firingEquipment = StartCoroutine(equipped.Fire(this)); }
    }

    public void InterruptEquipment(int damage)
    {
        if (equipped != null && firingEquipment != null && equipped.interruptable)
        {
            StopCoroutine(firingEquipment);
        }
    }
}
