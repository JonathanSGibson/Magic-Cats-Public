using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    string firstLevel = "Level 1";
    string fileDirectory;
    FileStream currentSave;

    [Serializable]
    class SaveData
    {
        public SaveData(string level_, int puzzlesComplete_)
        {
            level = level_;
            puzzlesComplete = puzzlesComplete_;
        }
        public string level;
        public int puzzlesComplete;
    }


    // Start is called before the first frame update
    void Start()
    {
        fileDirectory = Application.dataPath + "/SaveData.txt";
        currentSave = File.Open(fileDirectory, FileMode.OpenOrCreate);
        currentSave.Close();
    }

    public void Save(string level, int puzzlesComplete)
    {
        SaveData newSave = new SaveData(level, puzzlesComplete);

        string json = JsonUtility.ToJson(newSave);

        byte[] bytes = Encoding.ASCII.GetBytes(json); //Used this to heglp with converting to byte array https://www.c-sharpcorner.com/article/c-sharp-string-to-byte-array/
        int byteLength = Encoding.ASCII.GetByteCount(json);

        currentSave = File.Open(fileDirectory, FileMode.Open);
        currentSave.Flush();
        currentSave.Write(bytes, 0, byteLength);
        currentSave.Close();
    }

    public void Load()
    {
        currentSave = File.Open(fileDirectory, FileMode.Open);
        byte[] bytes = new byte[currentSave.Length];
        int bytesToRead = bytes.Length;
        currentSave.Read(bytes, 0, bytesToRead);
        currentSave.Close();

        string json = Encoding.ASCII.GetString(bytes);
        SaveData currentSaveData = JsonUtility.FromJson<SaveData>(json);

        PlayerPrefs.SetInt("PuzzlesComplete", currentSaveData.puzzlesComplete);
        SceneManager.LoadScene(currentSaveData.level);
        //When scene is loaded the save system in THAT scene runs its "awake" function using the playerpref just set
        //This could be handled by keeping one savesystem object between scenes instead
    }

    public void NewSave()
    {
        SaveData overwrite = new SaveData(firstLevel, 0);
        string json = JsonUtility.ToJson(overwrite);
        byte[] bytes = Encoding.ASCII.GetBytes(json);
        int byteLength = Encoding.ASCII.GetByteCount(json);
        currentSave = File.Open(fileDirectory, FileMode.Open);
        currentSave.Flush();
        currentSave.Write(bytes, 0, byteLength);
        currentSave.Close();
        PlayerPrefs.SetInt("PuzzlesComplete", 0);
        SceneManager.LoadScene(firstLevel);
    }

    private void Awake()
    {
        PuzzleOrbController orbController = GameObject.FindObjectOfType<PuzzleOrbController>();
        if (PlayerPrefs.GetInt("PuzzlesComplete") > 0 && orbController != null)
        {
            orbController.LoadLevel(PlayerPrefs.GetInt("PuzzlesComplete"));
        }
    }
}
