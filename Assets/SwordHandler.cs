using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordHandler : MonoBehaviour
{
    private InputAction swing;
    private AudioClip sound;
    [SerializeField] private CollideOnce bladeCollideOnce;

    private void Awake()
    {
        sound = Resources.Load<AudioClip>("Sounds/RPG_Essentials_Free/10_Battle_SFX/77_flesh_02");
    }
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
        //ClearQueue();
        GetComponent<Animator>().SetBool("SwingQueued", false);
        gameObject.SetActive(false);
    }

    void QueueSwing(InputAction.CallbackContext context)
    {
        GetComponent<Animator>().SetBool("SwingQueued", true);
    }

    void ClearQueue()
    {
        AudioSource.PlayClipAtPoint(sound, transform.position);
        bladeCollideOnce.Clear();
        GetComponent<Animator>().SetBool("SwingQueued", false);
    }
}
