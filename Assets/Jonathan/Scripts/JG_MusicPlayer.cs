using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource source;

    private void Awake()
    { 
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MusicPlayer");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetFloat("Music", 0.25f);

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        source.volume = PlayerPrefs.GetFloat("Music");
    }

    //private void OnLevelWasLoaded(int level)
    //{
    //    if (GameObject.FindGameObjectWithTag("MusicPlayer"))
    //        gameObject.SetActive(false);
    //    else
    //        DontDestroyOnLoad(this.gameObject);
    //}
}
