using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static List<Item> AvailableItems;

    void Start() {
        AvailableItems = Item.ReadXML();
    }

    void Update() {

    }


    public struct Config {
        public static string DEFAULT_ITEM_XML_PLACEMENT = ".\\Items.xml";
    }
}
