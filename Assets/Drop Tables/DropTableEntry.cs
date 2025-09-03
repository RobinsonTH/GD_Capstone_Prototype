using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class DropTableEntry : ScriptableObject
{
    [SerializeField] GameObject drop;
    [SerializeField] int weight;

    public GameObject GetDrop()
    {
        return drop;
    }

    public int GetWeight()
    {
        return weight;
    }
}
