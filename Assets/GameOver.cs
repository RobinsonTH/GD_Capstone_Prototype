using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Button defaultButton;

    [SerializeField] UnityEvent OnRetry;

    // Start is called before the first frame update
    void Start()
    {
        if(OnRetry == null)
        {
            OnRetry = new UnityEvent();
        }
    }

    private void OnEnable()
    {
        player.GetComponent<Health>().OnDeath += GameOverScreen;
    }

    private void OnDisable()
    {
        player.GetComponent<Health>().OnDeath -= GameOverScreen;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GameOverScreen()
    {
        EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void Retry()
    {
        OnRetry.Invoke();
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        player.gameObject.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Can't quit in editor.");
    }

    void OnApplicationQuit()
    {
        MonoBehaviour[] scripts = Object.FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
    }
}
