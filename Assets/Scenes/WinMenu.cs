using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    [SerializeField] GameObject winMenu;

    // Start is called before the first frame update
    void Start()
    {     
        // Disable the win menu when the game starts
        winMenu.SetActive(false);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Application has Quit.");
    }
}