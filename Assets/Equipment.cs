using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Equipment : ScriptableObject
{
    public bool interruptable = true;
    public abstract void OnEquip();
    public abstract void OnUnequip();
    public abstract IEnumerator Fire(Character source);
}
