using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ResourcePoint : MonoBehaviour {

    public string Resource = "Stone";
    [Range(1, 9999)]
    public int ResourceAmountGiven = 1;
    public int HitAmount = 5;
    public bool DestroyOnFinalHit = false;

    private Item item;
    private int hitCounter = 0;

    public void OnHit(PlayerController player) {
        hitCounter++;
        if (hitCounter >= HitAmount) {
            ItemStack itemStack = new ItemStack(item, ResourceAmountGiven);
            player.AddToInventory(itemStack);

            if (DestroyOnFinalHit) {
                Destroy(gameObject);
            }
        }
    }

    void Start() {
        item = GameController.GetItem(Resource);
    }

}
