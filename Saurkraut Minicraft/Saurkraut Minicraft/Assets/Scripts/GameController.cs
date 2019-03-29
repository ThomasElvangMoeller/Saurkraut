using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static List<Item> AvailableItems;


    public struct Config {
        public static string DEFAULT_ITEM_XML_PLACEMENT = ".\\Items.xml";
        public static string DEFAULT_PLAYER_INV_SAVE_PLACEMENT = ".\\Inventory.dat";
        public static string DEFAULT_ITEM_TEXTURE_FOLDER_PLACEMENT = ".\\ItemsTextures\\";
    }


    void Start() {
        AvailableItems = Item.ReadXML();
    }




    public static void SavePlayerInventory(List<Item> items) {
        SavePlayerInventory(items, Config.DEFAULT_PLAYER_INV_SAVE_PLACEMENT);
    }

    public static void SavePlayerInventory(List<Item> items, string place) {

        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();

        formatter.Serialize(stream, items);

        using (var file = new FileStream(place, FileMode.OpenOrCreate)) {

            byte[] invBuffer = stream.ToArray();

            file.Write(invBuffer, 0, invBuffer.Length);
        }
    }


    /// <summary>
    /// Takes in and fills the given list, using the default player inventory file placement
    /// </summary>
    /// <param name="items"></param>
    public static void LoadPlayerInventory(out List<Item> items) {
        LoadPlayerInventory(out items, Config.DEFAULT_PLAYER_INV_SAVE_PLACEMENT);
    }


    /// <summary>
    /// Takes in and fills the given list, using the given player inventory file placement
    /// </summary>
    /// <param name="items"></param>
    /// <param name="place"></param>
    public static void LoadPlayerInventory(out List<Item> items, string place) {

        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        int invBufferSize = 0;

        using (var file = new FileStream(place, FileMode.OpenOrCreate)) {
            while(file.ReadByte() != -1) {
                invBufferSize++;
            }
        }

        byte[] invBuffer = new byte[invBufferSize];

        using (var file = new FileStream(place, FileMode.OpenOrCreate)) {
            file.Read(invBuffer, 0, invBuffer.Length);
        }

        stream.Write(invBuffer, 0, invBuffer.Length);
        stream.Position = 0;

        items = formatter.Deserialize(stream) as List<Item>;
    }
}
