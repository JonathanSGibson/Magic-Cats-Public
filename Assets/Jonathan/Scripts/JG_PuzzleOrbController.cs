using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleOrbController : MonoBehaviour
{
    //Formatting for orb puzzle:
    //Attach this script to empty game object
    //Set the orbs to be it's children
    //Give it no other children
    List<GameObject> orbs;
    List<int> order;
    public List<Image> patternComplete;
    public Sprite patternCompleteSprite;
    public int patternCount = 1;
    public int patternLength = 5;
    public float glowTime = 0.5f; //in seconds
    public float correctGlowTime = 0.3f; //in seconds
    public float glowTimeGap = 0.5f; //in seconds
    public string successSceneName; //Scene to load on puzzle complete
    int puzzleStep = 0;
    int glowStep = 0;
    public int patternSuccesses;
    public GameObject toHide;
    public GameObject toShow;
    public Sprite correctSprite;
    public Sprite incorrectSprite;
    public float flashDuration; //How long to flash when level is complete/failed
    public GameObject startButton;
    public GameObject catController;
    public List<int> accessoryPoints; //Which puzzle completion step each accessory is gained at (0 counting)

    public AudioClip patternCmoplete;
    public AudioClip levelComplete;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        orbs = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            orbs.Add(transform.GetChild(i).gameObject);
            orbs[i].GetComponent<PuzzleOrb>().SetController(gameObject);
            Debug.Log("Added orb");
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartPuzzle()
    {
        startButton.SetActive(false);
        Debug.Log("Puzzle Started");
        glowStep = 0;
        puzzleStep = 0;

        //Set all orbs so clicking on them does nothing
        foreach(GameObject orb in orbs)
        {
            orb.GetComponent<PuzzleOrb>().SetPuzzleInactive();
        }

        //Create the order of the orbs
        order = new List<int>();
        for (int i = 0; i < patternLength; i++)
        {
            order.Add(Random.Range(0, orbs.Count));
        }

        orbs[order[0]].GetComponent<PuzzleOrb>().Glow();
        Debug.Log(order[0]);
    }

    public void NextPuzzleStep()
    {
        puzzleStep++;
        if (puzzleStep < patternLength)
        {
            //Sets whatever is the next orb in the sequence so it can be clicked
            orbs[order[puzzleStep]].GetComponent<PuzzleOrb>().SetOrbCorrect();
            Debug.Log("orb " + order[puzzleStep] + " correct");
        }
        else
        {
            foreach (int accessory in accessoryPoints)
            {
                if (patternSuccesses == accessory)
                {
                    catController.GetComponent<CatController>().ShowAccessory();
                    break;
                }
            }
            Debug.Log("\nCOMPLETE\n");
            patternComplete[patternSuccesses].sprite = patternCompleteSprite;

            foreach (GameObject orb in orbs)
            {
                orb.GetComponent<PuzzleOrb>().LevelCompleteFlash();
            }
            patternSuccesses++;
            foreach (GameObject orb in orbs)
            {
                orb.GetComponent<PuzzleOrb>().SetPuzzleInactive();
            }
            if (patternSuccesses >= patternCount)
            {
                //change UI elements to show one complete
                LevelComplete();
            }
            else
            {
                audioSource.PlayOneShot(patternCmoplete, PlayerPrefs.GetFloat("SFX"));
                startButton.SetActive(true);
            }
        }
    }

    public void NextGlowStep()
    {
        glowStep++;
        if (glowStep < patternLength)
        {
            orbs[order[glowStep]].GetComponent<PuzzleOrb>().Glow();
            Debug.Log(order[glowStep]);
        }
        else
        {
            foreach (GameObject orb in orbs)
            {
                orb.GetComponent<PuzzleOrb>().SetOrbIncorrect();
            }
            Debug.Log("orb " + order[0] + " correct");
            orbs[order[0]].GetComponent<PuzzleOrb>().SetOrbCorrect();
        }
    }

    public float GetGlowTimer()
    {
        return glowTime;
    }

    public float GetGlowGapTimer()
    {
        return glowTimeGap;
    }

    public float GetCorrectTimer()
    {
        return correctGlowTime;
    }

    void LevelComplete()
    {
        audioSource.PlayOneShot(levelComplete, PlayerPrefs.GetFloat("SFX"));
        startButton.SetActive(false);
        toHide.SetActive(false);
        toShow.SetActive(true);
    }

    public void PuzzleFailed()
    {
        foreach(GameObject orb in orbs)
        {
            orb.GetComponent<PuzzleOrb>().LevelFailedFlash();
            orb.GetComponent<PuzzleOrb>().SetPuzzleInactive();
        }
        startButton.SetActive(true);
    }

    public Sprite GetCorrectSprite()
    {
        return correctSprite;
    }

    public Sprite GetWrongSprite()
    {
        return incorrectSprite;
    }

    public float GetFlashDuration()
    {
        return flashDuration;
    }


    public void LoadLevel(int puzzlesComplete)
    {
        if (puzzlesComplete > 0)
        {
            patternSuccesses = 0;
            for (int i = 0; i < puzzlesComplete; i++)
            {
                foreach (int accessory in accessoryPoints)
                {
                    if (patternSuccesses == accessory)
                    {
                        catController.GetComponent<CatController>().ShowAccessory();
                        break;
                    }
                }
                Debug.Log("\nCOMPLETE\n");

                patternComplete[patternSuccesses].sprite = patternCompleteSprite;
                patternSuccesses++;
            }

            if (patternSuccesses >= patternCount)
            {
                //change UI elements to show one complete
                LevelComplete();
            }
            else
            {
                startButton.SetActive(true);
            }
        }
    }

    public void SaveLevel()
    {
        SaveSystem save = GameObject.FindObjectOfType<SaveSystem>();
        save.Save(SceneManager.GetActiveScene().name, patternSuccesses);
    }
}
