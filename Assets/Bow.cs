using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Bow : Equipment
{
    public Rigidbody2D projectile;

    public override void OnEquip()
    {
        Debug.Log("Equipped Bow");
    }

    public override void OnUnequip()
    {

    }

    public override IEnumerator Fire(Character source)
    {
        //lock controls/movement, then wait
        source.LoseControl();
        yield return new WaitForSeconds(0.33f);

        //Spawn an arrow, then give back control. Projectile behavior is kept in the arrow object.
        Instantiate(projectile, source.transform.position, source.transform.rotation);
        source.GainControl();
    }
}
