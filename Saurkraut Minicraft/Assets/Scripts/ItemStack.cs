using System;
using UnityEngine;

[Serializable]
public class ItemStack {

    public Item item { get; }
    public int itemAmount { get; set; }

    public ItemStack(Item item, int amount) {
        this.item = item;
        this.itemAmount = amount;
    }


    public override string ToString() {
        return item.ToString() + ", Amount: "+itemAmount;
    }
}
