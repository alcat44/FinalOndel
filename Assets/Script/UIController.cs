using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    bool UI = false;
    public GameObject Inventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UI == false)
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                Inventory.SetActive(true);
                UI = true;
            }
        }

        if (UI == true)
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                Inventory.SetActive(false);
                UI = false;
            }
        }
    }
}
