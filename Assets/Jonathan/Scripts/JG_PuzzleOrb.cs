using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PuzzleOrb : MonoBehaviour
{
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public AudioSource audioSource;


    float glowMaxTime;
    float glowTimer;
    float glowGapMaxTime;
    float glowGapTimer;
    bool glowing = false;
    bool glowingGap = false;
    bool correctGlow = false;
    float correctGlowTimer;
    float correctGlowMaxTime;


    bool flashing;
    float flashTimer;
    float flashDuration;

    Sprite wrongSprite;
    Sprite correctSprite;

    enum State
    {
        CorrectAnswer,
        WrongAnswer,
        Inactive
    }

    GameObject controller;
    State currentState = State.Inactive;
    Image orbImage;
    public Sprite orbDefault;
    public Sprite orbGlow;

    // Start is called before the first frame update
    void Start()
    {
        orbImage = GetComponent<Image>();
        orbImage.sprite = orbDefault;
    }

    // Update is called once per frame
    void Update()
    {
        if (glowing && Time.time >= glowTimer)
        {
            orbImage.sprite = orbDefault;
            glowing = false;
            glowingGap = true;
            glowGapTimer = Time.time + glowGapMaxTime;
        }
        else if (glowingGap && Time.time >= glowGapTimer)
        {
            glowingGap = false;
            controller.GetComponent<PuzzleOrbController>().NextGlowStep();
        }
        else if(correctGlow && Time.time >= correctGlowTimer)
        {
            CorrectAfterWait();
        }
        else if (flashing && Time.time >= flashTimer)
        {
            FlashEnd();
        }
    }


    public void SetController(GameObject newController)
    {
        controller = newController;
        glowMaxTime = controller.GetComponent<PuzzleOrbController>().GetGlowTimer();
        glowGapMaxTime = controller.GetComponent<PuzzleOrbController>().GetGlowGapTimer();
        correctGlowMaxTime = controller.GetComponent<PuzzleOrbController>().GetCorrectTimer();

        wrongSprite = controller.GetComponent<PuzzleOrbController>().GetWrongSprite();
        correctSprite = controller.GetComponent<PuzzleOrbController>().GetCorrectSprite();

        flashDuration = controller.GetComponent<PuzzleOrbController>().GetFlashDuration();
    }

    public void OnClick()
    {
        //When the orb is clicked checks what state it is and take action accordingly
        switch(currentState)
        {
            case State.CorrectAnswer:
                Correct();
                break;
            case State.WrongAnswer:
                Incorrect();
                break;
        }
    }

    //If the orb is clicked and is correct, set the orb to be the "wrong" orb and make the controller take the next step in the puzzle
    public void Correct()
    {
        Debug.Log("GlowStart");

        currentState = State.Inactive;
        orbImage.sprite = correctSprite;
        correctGlowTimer = Time.time + correctGlowMaxTime;
        correctGlow = true;
        audioSource.PlayOneShot(correctSound, PlayerPrefs.GetFloat("SFX"));
    }

    public void CorrectAfterWait()
    {
        Debug.Log("GlowEnd");
        correctGlow = false;
        orbImage.sprite = orbDefault;
        currentState = State.WrongAnswer;
        controller.GetComponent<PuzzleOrbController>().NextPuzzleStep();
    }

    //If you click the wrong orb the puzzle resets itself
    public void Incorrect()
    {
        Debug.Log("Incorrect");
        //Probably add in a visual effect for failing
        controller.GetComponent<PuzzleOrbController>().PuzzleFailed();
        audioSource.PlayOneShot(incorrectSound, PlayerPrefs.GetFloat("SFX"));
    }




    public void Glow()
    {
        orbImage.sprite = orbGlow;
        glowTimer = Time.time + glowMaxTime;
        glowing = true;
        audioSource.PlayOneShot(correctSound, PlayerPrefs.GetFloat("SFX"));
    }



    public void LevelCompleteFlash()
    {
        flashTimer = Time.time + flashDuration;
        orbImage.sprite = correctSprite;
        flashing = true;
    }

    public void LevelFailedFlash()
    {
        glowing = false;
        glowingGap = false;
        flashTimer = Time.time + flashDuration;
        orbImage.sprite = wrongSprite;
        flashing = true;
    }

    void FlashEnd()
    {
        orbImage.sprite = orbDefault;
        flashing = false;
    }





    //Public functions to change the enum, used by the puzzle controller
    public void SetPuzzleInactive()
    {
        currentState = State.Inactive;
    }

    public void SetOrbCorrect()
    {
        currentState = State.CorrectAnswer;
    }

    public void SetOrbIncorrect()
    {
        currentState = State.WrongAnswer;
    }


}
