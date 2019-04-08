using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>GameController must be one of the first scripts that has its Start() called, as it loads the neccesary resources from the disk.</para>
/// <para>This is done in Edit -> Project Settings -> Script Execution Order. </para>
/// <para>Add GameController to the list and move it to before defaults </para>
/// </summary>
public class GameController : MonoBehaviour {

    public static List<Item> AvailableItems;
    public static List<CraftingRecipe> AvailableCraftingRecipes;


    public struct Config {
        public static int DEFAULT_PLAYER_INVENTORY_SIZE = 24;
        public static string DEFAULT_ITEM_XML_PLACEMENT = ".\\Items.xml";
        public static string DEFAULT_CRAFTING_RECIPES_XML_PLACEMENT = ".\\CraftingRecipes.xml";
        public static string DEFAULT_PLAYER_INV_SAVE_PLACEMENT = ".\\Inventory.dat";
        public static string DEFAULT_ITEM_TEXTURE_FOLDER_PLACEMENT = ".\\Assets\\Sprites\\ItemsTextures\\";
    }


    void Start() {
        //Available items MUST be instantiated before crafting recipes as it is dependent on AvailableItems
        AvailableItems = Item.ReadXML();
        AvailableCraftingRecipes = CraftingRecipe.ReadXML();
        /*
        foreach (var item in AvailableCraftingRecipes) {
            print(item);
        }
        print(AvailableCraftingRecipes.Count);
        */
        //print(Config.DEFAULT_ITEM_TEXTURE_FOLDER_PLACEMENT + AvailableItems[0].ImageName);
    }

    public static Item GetItem(string name) {
        foreach (Item item in AvailableItems) {
            if (item.Name == name) {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// Gives a list of recipes that are possible with the itemstacks in the inventory given in the parameter
    /// </summary>
    /// <param name="inventory"></param>
    /// <returns></returns>
    public static List<CraftingRecipe> PossibleRecipes(List<ItemStack> inventory) {

        List<CraftingRecipe> tmp = new List<CraftingRecipe>();

        if (AvailableCraftingRecipes != null) {
            foreach (CraftingRecipe recipe in AvailableCraftingRecipes) {
                if (recipe.CraftingPossible(inventory)) {
                    tmp.Add(recipe);
                }
            }
        }
        return tmp;
    }

    /// <summary>
    /// Saves the given list, using the default player inventory file placement
    /// </summary>
    /// <param name="items"></param>
    public static void SavePlayerInventory(List<ItemStack> items) {
        SavePlayerInventory(items, Config.DEFAULT_PLAYER_INV_SAVE_PLACEMENT);
    }

    /// <summary>
    /// Saves the given list, using the given player inventory file placement
    /// </summary>
    /// <param name="items"></param>
    /// <param name="place"></param>
    public static void SavePlayerInventory(List<ItemStack> items, string place) {

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
    public static void LoadPlayerInventory(out List<ItemStack> items) {
        LoadPlayerInventory(out items, Config.DEFAULT_PLAYER_INV_SAVE_PLACEMENT);
    }


    /// <summary>
    /// Takes in and fills the given list, using the given player inventory file placement
    /// </summary>
    /// <param name="items"></param>
    /// <param name="place"></param>
    public static void LoadPlayerInventory(out List<ItemStack> items, string place) {

        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();

        byte[] invBuffer;

        invBuffer = File.ReadAllBytes(place);

        stream.Write(invBuffer, 0, invBuffer.Length);
        stream.Position = 0;

        items = formatter.Deserialize(stream) as List<ItemStack>;



        foreach (var item in items) {
            item.item = GetItem(item.item.Name);
        }
    }
}
