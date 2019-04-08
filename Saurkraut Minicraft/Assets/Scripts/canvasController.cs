using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvasController : MonoBehaviour {
    private Transform inventoryPanel;
    public Transform prefabItem;

    void Start() {
        GameController.LoadPlayerInventory(out PlayerController.Inventory);
        inventoryPanel = GameObject.Find("inventoryPanel").transform;
        updateInventory();
        PlayerController.InventoryChanged += updateInventory;
    }

    private void OnEnable() {
        updateInventory();
    }



    void updateInventory() {


        for (int i = 0; i < PlayerController.Inventory.Count; i++) {
            Transform slot = inventoryPanel.GetChild(i);

            if (slot.childCount > 0) {
                var c = slot.GetChild(0);
                if (c != null) {
                    Destroy(c.gameObject);
                }
            }

            if (PlayerController.Inventory[i] != null) {
                var creationOfRandomItems = Instantiate(prefabItem);
                creationOfRandomItems.SetParent(slot);
                creationOfRandomItems.GetChild(0).GetComponent<Text>().text = PlayerController.Inventory[i].itemAmount + "";
                creationOfRandomItems.GetComponent<Image>().sprite = PlayerController.Inventory[i].item.GetItemImageAsSprite();
            }
        }
    }
}
