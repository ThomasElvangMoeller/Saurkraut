using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    List<Item> Items;


    void Start() {

        //Lav så den loader Items fra et andet sted, hvis man skifter scene
        Items = Item.ReadXML();
        foreach (var item in Items) {
            print(item);
            print(item.Name);
        }
        
    }

    void Update()
    {
        
    }
}
