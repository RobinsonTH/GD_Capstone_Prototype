using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathDroptable : MonoBehaviour
{
    [SerializeField] DropTable table;
    private Health health;

    // Start is called before the first frame update
    void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.OnDeath += Roll;
    }

    private void OnDisable()
    {
        health.OnDeath -= Roll;
    }

    private void Roll()
    {
        GameObject roll = table.RollTable();
        if (roll != null)
        {
            Instantiate(roll, transform.position, Quaternion.identity, transform.parent);
            //Instantiate(roll);
        }
    }
}
