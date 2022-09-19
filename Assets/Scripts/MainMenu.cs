using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject creditsMenu;
   
    public void PlayGame()
    {
        SceneManager.LoadScene("Main_Game");
    }

    public void ToMainMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void ToSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }
    
    List<int> widths = new List<int>() { 1920, 1280, 960, 568 };
    List<int> heights = new List<int>() { 1080, 800, 540, 330 };
    public void SetResolutionSize(int index)
    {
        bool fullscreen = Screen.fullScreen;
        int width = widths[index];
        int height = heights[index];
        Screen.SetResolution(width, height,fullscreen);
    }

    public void ToCredits()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }


    public void Quit()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }
}
