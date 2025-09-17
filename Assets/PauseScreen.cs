using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Dungeon dungeon;
    [SerializeField] private Camera mapCamera;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button defaultButton;
    [SerializeField] private AudioSource pauseClip;
    [SerializeField] private AudioSource resumeClip;
    private InputAction pause;
    private InputAction resume;
    private InputAction confirm;

    //private InputActionMap playerMap;
    //private InputActionMap pauseMap;

    private void Awake()
    {
        //InputManager.Input = input;
        //inputActions = new PlayerInputActions();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        InputManager.Actions.PauseScreen.Disable();
    }

    private void OnEnable()
    {
        pause = InputManager.Actions.Player.Pause;
        pause.Enable();
        pause.performed += Pause;

        resume = InputManager.Actions.PauseScreen.Resume;
        resume.Enable();

        confirm = InputManager.Actions.UI.Submit;
        confirm.Disable();
        //resume.performed += Resume;
    }

    private void OnDisable()
    {
        pause.Disable();
        resume.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause(InputAction.CallbackContext context)
    {

        //InputManager.Input.SwitchCurrentActionMap("PauseScreen");
        InputManager.Actions.Player.Disable();
        InputManager.Actions.PauseScreen.Enable();
        pausePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
        if (mapCamera != null)
        {
            dungeon.UnmaskExploredRooms();
            mapCamera.gameObject.SetActive(true);
        }
        Time.timeScale = 0.0f;
        pauseClip.Play();
        pause.performed -= Pause;
        resume.performed += Resume;
        StartCoroutine(PauseDelay());
        //Time.timeScale = Time.timeScale * -1 + 1
        //Debug.Log("paused");
    }

    public void Resume(InputAction.CallbackContext context)
    {
        ResumeGame();
    }

    public void ResumeGame()
    {
        InputManager.Actions.Player.Enable();
        InputManager.Actions.PauseScreen.Disable();
        if(mapCamera != null)
        {
            dungeon.MaskRooms();
            mapCamera.gameObject.SetActive(false);
        }
        
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
        resumeClip.Play();
        pause.performed += Pause;
        resume.performed -= Resume;
        StartCoroutine(PauseDelay());
    }

    private IEnumerator PauseDelay()
    {
        yield return new WaitForSeconds(0.1f);
        if (!confirm.enabled)
        {
            confirm.Enable();
        }
        else
        {
            confirm.Disable();
        }
    }
    
}
