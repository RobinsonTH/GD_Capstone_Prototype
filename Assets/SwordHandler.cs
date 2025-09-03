using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordHandler : MonoBehaviour
{
    private InputAction swing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        swing = InputManager.Actions.Player.Swing;
        swing.Enable();
        swing.performed += QueueSwing;

    }

    private void OnDisable()
    {
        swing.performed -= QueueSwing;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PutAway()
    {
        ClearQueue();
        gameObject.SetActive(false);
    }

    void QueueSwing(InputAction.CallbackContext context)
    {
        GetComponent<Animator>().SetBool("SwingQueued", true);
    }

    void ClearQueue()
    {
        GetComponent<Animator>().SetBool("SwingQueued", false);
    }
}
