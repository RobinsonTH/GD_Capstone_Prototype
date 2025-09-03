using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DropTable : ScriptableObject
{
    [SerializeField] List<DropTableEntry> drops;
    [SerializeField] int whiffWeight;

    public GameObject RollTable()
    {
        int tableLength = whiffWeight;
        foreach (DropTableEntry entry in drops)
        {
            tableLength += entry.GetWeight();
        }

        int roll = UnityEngine.Random.Range(0, tableLength);
        int weightIndex = 0;
        foreach (DropTableEntry entry in drops)
        {
            if(roll < (entry.GetWeight() + weightIndex))
            {
                return entry.GetDrop();
            }
            weightIndex += entry.GetWeight();
        }
        return null;
    }

}

