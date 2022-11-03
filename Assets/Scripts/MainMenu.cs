using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject settingsMenu;
    [SerializeField]
    private GameObject creditsMenu;

    //Start the game
    public void PlayGame()
    {
        SceneManager.LoadScene("Main_Game");
    }

    //Opens Main Menu tab
    public void ToMainMenu()
    {
        InactiveAllMenus();
        mainMenu.SetActive(true);
    }

    //Opens Settings tab
    public void ToSettings()
    {
        InactiveAllMenus();
        settingsMenu.SetActive(true);
    }
    //Opens Credits tab
    public void ToCredits()
    {
        InactiveAllMenus();
        creditsMenu.SetActive(true);
    }
    //Sets all menus inactive by default
    private void InactiveAllMenus()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }
    //Add resolutions
    [HideInInspector]
    public List<Vector2> resolutions = new List<Vector2>() {
        new Vector2(3840, 2160),
        new Vector2(3440, 1440),
        new Vector2(2560, 1440),
        new Vector2(1920, 1080),
        new Vector2(1600, 768),
        new Vector2(1366, 768),
        new Vector2(1280, 1024)
    };

    //Set the resolution
    public void SetResolutionSize(int index)
    {
        Vector2 resolution = resolutions[index];
        Screen.SetResolution((int)resolution.x, (int)resolution.y, Screen.fullScreen);
    }

    //Exits application
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }
}