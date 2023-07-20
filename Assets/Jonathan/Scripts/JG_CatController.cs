using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    //First list represents the accessories ON THE CAT, they should be deactive by default
    public List<GameObject> catAccessories;
    //Second list represents the accessories by the indicators of how far through the puzzle you are, they should be active by default
    public List<GameObject> accessoryIndicators;
    int accessoriesShown = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowAccessory()
    {
        catAccessories[accessoriesShown].SetActive(true);
        accessoryIndicators[accessoriesShown].SetActive(false); //This should probably be replaced with greying out but I am deactivating for testing purposes
        accessoriesShown++;
    }
}
