using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject creditsScreen;
    [SerializeField] List<Graphic> lines;
    [SerializeField] List<Button> buttons;
    [SerializeField] Button creditsButton;
    [SerializeField] Color lineColor;

    [SerializeField] float fadeDuration;

    // Start is called before the first frame update
    void Start()
    {
        

        foreach(Graphic line in lines)
        {
            line.color = lineColor;
        }

        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }

        StartCoroutine(FadeInText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FadeInText()
    {
        yield return new WaitForSeconds(fadeDuration);
        foreach (Graphic line in lines)
        {
            float t = 0;
            while (t <= fadeDuration)
            {
                lineColor.a = (t / fadeDuration);
                line.color = lineColor;
                t += Time.deltaTime;
                yield return null;
            }
        }
        EnableButtons();
    }

    private void EnableButtons()
    {
        foreach(Button button in buttons)
        {
            button.gameObject.SetActive(true);
        }

    }

    public void ReplayGame()
    {
        SceneManager.LoadScene("Dungeon1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleCredits()
    {
        titleScreen.SetActive(!titleScreen.activeSelf);
        creditsScreen.SetActive(!creditsScreen.activeSelf);
        if (creditsScreen.activeSelf) { EventSystem.current.SetSelectedGameObject(creditsButton.gameObject); }
        else { EventSystem.current.SetSelectedGameObject(buttons[0].gameObject); }
    }
}
