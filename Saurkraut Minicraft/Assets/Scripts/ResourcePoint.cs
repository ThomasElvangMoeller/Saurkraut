using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePoint : MonoBehaviour {

    public string Resource = "Stone";
    [Range(1, 9999)]
    public int ResourceAmountGiven = 1;

    private Item item;

    public void OnHit(PlayerController player) {
        ItemStack itemStack = new ItemStack(item, ResourceAmountGiven);
        player.AddToInventory(itemStack);
    }

    void Start() {
        item = GameController.GetItem(Resource);
    }

}
