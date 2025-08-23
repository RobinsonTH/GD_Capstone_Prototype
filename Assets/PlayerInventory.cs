using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int maxArrows;
    [SerializeField] private int arrows;
    [SerializeField] private TextMeshProUGUI arrowCounter;

    void Update()
    {
        arrowCounter.text = "× " + arrows.ToString();
    }

    public void GainArrows(int amount)
    {
        arrows += amount;
        if(arrows > maxArrows)
        {
            arrows = maxArrows;
        }
    }

    public int GetArrows()
    {
        return arrows;
    }

    public void ShootArrow()
    {
        if (arrows > 0)
        {
            arrows--;
        }
    }
}
