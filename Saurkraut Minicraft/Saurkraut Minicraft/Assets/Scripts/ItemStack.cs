using UnityEngine;

public class ItemStack : MonoBehaviour {

    [HideInInspector]
    public Item item { get; }
    [HideInInspector]
    public int itemAmount { get; set; }

    public ItemStack(Item item, int amount) {
        this.item = item;
        this.itemAmount = amount;
    }
}
