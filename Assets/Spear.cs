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
        source.LoseControl();
        rb.velocity = source.transform.up * 10;
        while(rb.velocity.magnitude > 0)
        {
            source.HoldControl();
            yield return null;
        }
        float t = 0f;
        while(t < 3.0f)
        {
            source.HoldControl();
            t += Time.deltaTime;
            yield return null;
        }
        GameObject.Destroy(spear);
        source.GainControl();
    }
}
