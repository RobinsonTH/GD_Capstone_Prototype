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
    private InputAction pause;
    private InputAction resume;

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
        resume.performed += Resume;
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
    }

    
}
