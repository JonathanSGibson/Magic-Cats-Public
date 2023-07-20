using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPage : MonoBehaviour
{

    public Slider SFX;
    public Slider music;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("SFX"))
            PlayerPrefs.SetFloat("SFX", 0.25f);

        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetFloat("Music", 0.25f);

        SFX.value = PlayerPrefs.GetFloat("SFX");
        music.value = PlayerPrefs.GetFloat("Music");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSFXVolume()
    {
        PlayerPrefs.SetFloat("SFX", SFX.value);
    }

    public void UpdateMusicVolume()
    {
        PlayerPrefs.SetFloat("Music", music.value);
    }
}
