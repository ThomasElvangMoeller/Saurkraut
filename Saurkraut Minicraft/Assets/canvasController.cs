using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvasController : MonoBehaviour
{
    private Transform inventoryPanel;
    public Transform prefabItem;
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        GameController.LoadPlayerInventory(out PlayerController.Inventory);
        inventoryPanel = gameObject.transform.Find("inventoryPanel");
        updateInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateInventory()
    {
        int indexnr = 0;
        var currentPlayerInventory = PlayerController.Inventory;
        foreach (Transform slot in inventoryPanel)
        {
            if(indexnr > 0)
            {
                if(currentPlayerInventory[indexnr-1] != null) { 
                    var creationOfRandomItems = Instantiate(prefabItem);
                    creationOfRandomItems.parent = slot;
                    creationOfRandomItems.GetComponent<Image>().sprite = currentPlayerInventory[indexnr - 1].item.GetItemImageAsSprite();
                    creationOfRandomItems.GetChild(0).GetComponent<Text>().text = currentPlayerInventory[indexnr - 1].itemAmount + "";
                }

            }

            indexnr++;
        }
    }
}
