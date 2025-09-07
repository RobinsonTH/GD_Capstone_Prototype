using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class ChargingSpear : Equipment
{
    [SerializeField] GameObject weapon;
    public override void OnEquip()
    {

    }

    public override void OnUnequip()
    {
        
    }

    public override IEnumerator Fire(Character source)
    {
        Rigidbody2D rb = source.GetComponent<Rigidbody2D>();

        GameObject spear = Instantiate(weapon, source.transform);
        //source.LoseControl();
        rb.velocity = source.transform.up * 10;
        while(rb.velocity.magnitude > 0)
        {
            if(!source.GetControl())
            {
                rb.velocity = Vector3.zero;
            }
            yield return null;
        }
        float t = 0f;
        while (spear != null && t < 3.0f)
        {
            t += Time.deltaTime;
            yield return null;
        }

        //source.GainControl();
        GameObject.Destroy(spear);
    }
}
