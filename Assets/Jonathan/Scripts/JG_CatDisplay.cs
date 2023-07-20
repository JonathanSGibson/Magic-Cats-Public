using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.IO;
public class CatDisplay : MonoBehaviour
{
    string fileDirectory;
    FileStream loadedSave;
    public List<GameObject> accessories;
    public List<GameObject> cats;

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
        loadedSave = File.Open(fileDirectory, FileMode.OpenOrCreate);
        loadedSave.Close();
        LoadCats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int AddAccessories(int levelsComplete)
    {
        if (levelsComplete >= 5)
            return 3;
        else if (levelsComplete >= 3)
            return 2;
        else if (levelsComplete >= 1)
            return 1;
        else
            return 0;
    }

    public void LoadCats()
    {
        loadedSave = File.Open(fileDirectory, FileMode.Open);
        byte[] bytes = new byte[loadedSave.Length];
        int bytesToRead = bytes.Length;
        loadedSave.Read(bytes, 0, bytesToRead);
        loadedSave.Close();
        SaveData currentSaveData;
        if (bytesToRead <= 0)
        {
            currentSaveData = new SaveData("Level 1", 0);
            string json2 = JsonUtility.ToJson(currentSaveData);
            byte[] bytes2 = Encoding.ASCII.GetBytes(json2);
            int byteLength = Encoding.ASCII.GetByteCount(json2);
            FileStream currentSave = File.Open(fileDirectory, FileMode.Open);
            currentSave.Flush();
            currentSave.Write(bytes2, 0, byteLength);
            currentSave.Close();
            PlayerPrefs.SetInt("PuzzlesComplete", 0);
        }
        else
        {

            string json = Encoding.ASCII.GetString(bytes);
             currentSaveData = JsonUtility.FromJson<SaveData>(json);
        }



        int accessoriesToLoad = 0;
        int cat = 0;

        //This is gross and hardcoded but is the easiest solution without completely overhalling the existing level system
        switch (currentSaveData.level)
        {
            case "Level 1":
                accessoriesToLoad += currentSaveData.puzzlesComplete;
                break;
            case "Level 2":
                accessoriesToLoad += 3;
                cat += 1;
                accessoriesToLoad += currentSaveData.puzzlesComplete;
                break;
            case "Level 3":
                accessoriesToLoad += 3*2;
                accessoriesToLoad += AddAccessories(currentSaveData.puzzlesComplete);
                cat += 2;
                break;
            case "Level 4":
                accessoriesToLoad += 3*3;
                cat += 3;
                accessoriesToLoad += AddAccessories(currentSaveData.puzzlesComplete);
                break;
            case "Level 5":
                accessoriesToLoad += 3*4;
                cat += 4;
                accessoriesToLoad += AddAccessories(currentSaveData.puzzlesComplete);
                break;
            case "Level 6":
                accessoriesToLoad += 3*5;
                cat += 5;
                accessoriesToLoad += AddAccessories(currentSaveData.puzzlesComplete);
                break;
            case "Level 7":
                accessoriesToLoad += 3*6;
                cat += 6;
                accessoriesToLoad += AddAccessories(currentSaveData.puzzlesComplete);
                break;
                
        } 



        for (int i = 0; i < accessoriesToLoad; i++)
        {
            accessories[i].SetActive(true);
        }

        for (int i = 0; i <= cat; i++)
        {
            cats[i].SetActive(true);
        }
    }
}
