using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDisableEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent Event;

    private void Start()
    {
        if (Event == null)
            Event = new UnityEvent();
    }

    private void OnDisable()
    {
        Event.Invoke();
    }
}
