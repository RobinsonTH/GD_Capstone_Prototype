using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDestroyEvent : MonoBehaviour
{
    [SerializeField] UnityEvent Event;
    // Start is called before the first frame update
    void Start()
    {
        if (Event == null)
            Event = new UnityEvent();

        //Event.AddListener(OnEventTriggered);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if(Event != null)
        {
            Event.Invoke();
        }
    }
}
