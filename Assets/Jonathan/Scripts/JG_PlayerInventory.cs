using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public int inventorySize = 1;
    //public Sprite inventoryBackground;
    //public Vector2 inventoryTopLeftCorner;
    //public int itemsPerRow;

    public Sprite testItem;


    public enum Element
    {
        EMPTY,
        FIRE,
        WATER,
        NATURE,
        EARTH,
        LIGHT,
        DARK,
        SPIRIT
    }

   public  enum ItemType
    {
        EMPTY,
        SPORE,
        MUSHROOM,
        POTION
    }


    public struct Item
    {
        public ItemType type;
        public Element element;
        public Sprite sprite;

        public Item(ItemType type_ = ItemType.EMPTY, Element element_ = Element.EMPTY, Sprite sprite_ = null)
        { 
            type = type_;
            element = element_;
            sprite = sprite_;
        }
    }

    //Currently inventory is set up as a single array, if specific inventory sizes were wanted for spores, mushrooms and potions seperately this would have to be split into several arrays
    public Item[] inventory;




    // Start is called before the first frame update
    void Start()
    {
        List<Image> temp = new List<Image>();
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("InventoryItem"))
                temp.Add(child.GetComponent<Image>());
        }

        inventorySize = temp.Count;
        inventory = new Item[inventorySize];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SortInventory()
    {
        //Sort by itemType
        //Then sort by element
        List<Item> spores = new List<Item>();
        List<Item> mushrooms = new List<Item>();
        List<Item> potions = new List<Item>();

        foreach (Item item in inventory)
        {
            switch(item.type)
            {
                case ItemType.SPORE:
                    spores.Add(item);
                    break;
                case ItemType.MUSHROOM:
                    mushrooms.Add(item);
                    break;
                case ItemType.POTION:
                    potions.Add(item);
                    break;
            }
        }
        //Quick sort each based on element
        //Recompile list
        spores = QuickSort(spores, 0, spores.Count - 1);
        mushrooms = QuickSort(mushrooms, 0, mushrooms.Count - 1);
        potions = QuickSort(potions, 0, potions.Count - 1);


        Array.Clear(inventory, 0, inventorySize);
        int count = 0;

        foreach(Item item in spores)
        {
            inventory[count] = item;
            count++;
        }
        foreach (Item item in mushrooms)
        {
            inventory[count] = item;
            count++;
        }
        foreach (Item item in potions)
        {
            inventory[count] = item;
            count++;
        }
    }

    List<Item> QuickSort(List<Item> input, int first, int last)
    {
        if (first<last)
        {
            int pivotValue = (int)input[first].element;
            int leftPointer = first;
            int rightPointer = last;

            while (leftPointer < rightPointer)
            {
                while ((int)input[leftPointer].element <= pivotValue && leftPointer <= rightPointer && leftPointer < last)
                {
                    leftPointer++;
                }
                while((int)input[rightPointer].element >= pivotValue && rightPointer >= leftPointer && rightPointer > first)
                {
                    rightPointer--;
                }
                if (leftPointer < rightPointer)
                {
                    Item temp = input[leftPointer];
                    input[leftPointer] = input[rightPointer];
                    input[rightPointer] = temp;
                }
            }

            int pivot = rightPointer;
            Item temp1 = input[first];
            input[first] = input[pivot];
            input[pivot] = temp1;

            input = QuickSort(input, first, pivot - 1);
            input = QuickSort(input, pivot+1, last);
        }
        return input;
    }

    public void RemoveItem(ItemType type, Element element)
    {
        int count = 0;
        foreach (Item currentItem in inventory)
        {
            if (currentItem.type == type && currentItem.element == element)
                inventory[count] = new Item();
            count++;
        }
    }

    //Returns true if item is added, returns false if there is not space in the inventory
    public bool AddItem(Item item)
    {
        int count = 0;
        foreach (Item currentItem in inventory)
        {
            if (currentItem.element == Element.EMPTY && currentItem.type == ItemType.EMPTY)
            {
                inventory[count] = item;
                return true;
            }
            count++;
        }
        return false;
    }

    public bool CheckItem(ItemType type, Element element)
    {
        foreach (Item currentItem in inventory)
        {
            if (currentItem.type == type && currentItem.element == element)
                return true;
        }
        return false;
    }

    public Element FindFirstItem(ItemType type)
    {
        foreach (Item currentItem in inventory)
        {
            if (currentItem.type == type)
                return currentItem.element;
        }
        return Element.EMPTY;
    }

    public int CheckItemCount(ItemType type, Element element)
    {
        int count = 0;
        foreach (Item currentItem in inventory)
        {
            if (currentItem.type == type && currentItem.element == element)
                count++;
        }
        return count;
    }


    public void DrawInventory()
    {
        List<Image> temp = new List<Image>();
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("InventoryItem"))
                temp.Add(child.GetComponent<Image>());
        }

        for (int i = 0; i < inventory.Length; i++)
        {
            if (!(inventory[i].element == Element.EMPTY || inventory[i].type == ItemType.EMPTY))
            {
                temp[i].sprite = inventory[i].sprite;
            }
        }
    }



    public void AddItemTest()
    {
        AddItem(new Item(ItemType.MUSHROOM, Element.FIRE, testItem));
    }

    ///Need to transfer this between scenes
}
