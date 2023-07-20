using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject nonPauseMenu;
    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (GameIsPaused)
        //    {
        //        Resume();
        //    } 
        //    else
        //    {
        //        Pause();
        //    }
        //}
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        nonPauseMenu.SetActive(false);
        GameIsPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        nonPauseMenu.SetActive(true);
        GameIsPaused = false;
    }

    public void ExitButton()
    {
        nonPauseMenu.SetActive(true);
        PuzzleOrbController orbController = GameObject.FindObjectOfType<PuzzleOrbController>();
        if (orbController != null)
            orbController.SaveLevel();
        Application.Quit();
        Debug.Log("Quit");
    }

    public void MainMenu()
    {
        nonPauseMenu.SetActive(true);
        PuzzleOrbController orbController = GameObject.FindObjectOfType<PuzzleOrbController>();
        if (orbController != null)
            orbController.SaveLevel();
        SceneManager.LoadScene("JacksTempMenu");
    }

    public void OnPause()
    {
        if (GameIsPaused)
        {
            Resume();
        } 
        else
        {
            Pause();
        }
    }
    

}
