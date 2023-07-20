using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LevelSelect(string Level)
    {
        Debug.Log("Load scene " + Level);
        SceneManager.LoadScene(Level);
    }

    public void ExitButton ()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
