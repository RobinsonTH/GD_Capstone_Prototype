using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Bow : Equipment
{
    public Rigidbody2D projectile;
    public float delaySeconds;

    public override void OnEquip()
    {
        //Debug.Log("Equipped Bow");
    }

    public override void OnUnequip()
    {

    }

    public override IEnumerator Fire(Character source)
    {
        if(source.GetComponent<PlayerInventory>() &&  source.GetComponent<PlayerInventory>().GetArrows() <= 0)
        {
            yield break;
        }

        //lock controls/movement, then wait
        source.LoseControl();
        yield return new WaitForSeconds(delaySeconds);

        //Spawn an arrow, then give back control. Projectile behavior is kept in the arrow object.
        Instantiate(projectile, source.transform.position, source.transform.rotation);

        if (source.GetComponent<PlayerInventory>())
        {
            source.GetComponent<PlayerInventory>().ShootArrow();
        }

        source.GainControl();
    }
}
