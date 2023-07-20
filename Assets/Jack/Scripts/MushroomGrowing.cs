using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MushroomGrowing : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite sprite;

    public enum States
    {
        EMPTY,
        GROWING,
        GROWN
    }

    States currentState;
    float timerEnd;
    public float timerDuration = 1;
    public GameObject playerInventory;

    PlayerInventory.Item currentItem;

    public void OnClick()
    {
        if (currentState == States.GROWN)
        {
            if (playerInventory.GetComponent<PlayerInventory>().AddItem(currentItem))
            {
                currentState = States.EMPTY;
            }
        }
        else if (currentState == States.EMPTY)
        {
            PlayerInventory.Element temp = playerInventory.GetComponent<PlayerInventory>().FindFirstItem(PlayerInventory.ItemType.SPORE);
            if (temp != PlayerInventory.Element.EMPTY)
            {
                playerInventory.GetComponent<PlayerInventory>().RemoveItem(PlayerInventory.ItemType.SPORE, temp);
                currentItem = new PlayerInventory.Item(PlayerInventory.ItemType.MUSHROOM, temp, sprite);
                currentState = States.GROWING;
                timerEnd = Time.time + timerDuration;
            }

            
        }
    }

    void Start()
    {
        currentState = States.EMPTY;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == States.GROWING || timerEnd <= Time.time)
        {
            currentState = States.GROWN;
        }
        
    }

    
}
