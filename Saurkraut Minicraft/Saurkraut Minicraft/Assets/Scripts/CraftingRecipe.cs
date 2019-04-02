using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using UnityEngine.WSA;

public class CraftingRecipe {

    List<ItemStack> Input;
    List<ItemStack> Output;

    public CraftingRecipe(List<ItemStack> input, List<ItemStack> output) {
        this.Input = input;
        this.Output = output;
    }

    public CraftingRecipe(List<ItemStack> input, Item output) {
        this.Input = input;
        this.Output = new List<ItemStack> { new ItemStack(output, 1) };
    }

    public bool CraftingPossible(List<ItemStack> inventory) {
        if (ContainsAllItems(inventory)) { 
            return true;
        } else {
            return false;
        }
    }

    private bool HasRoom() {
        return (GameController.Config.DEFAULT_PLAYER_INVENTORY_SIZE - Input.Count + Output.Count) <= GameController.Config.DEFAULT_PLAYER_INVENTORY_SIZE;
    }

    

    private bool ContainsAllItems(List<ItemStack> inventory) {
        int count = 0;
        foreach (ItemStack item in Input) {
            ItemStack tmp = inventory.FindFromName(item.item.Name);
            if(tmp != null && tmp.itemAmount >= item.itemAmount) {
                count++;
            }
        }
        return count >= Input.Count;
    }


    public void Craft(ref List<ItemStack> inventory) {
        if (HasRoom() && ContainsAllItems(inventory)) {
            foreach (ItemStack item in Input) {
                if (inventory.FindFromName(item.item.Name).itemAmount > item.itemAmount) {
                    inventory.FindFromName(item.item.Name).itemAmount -= item.itemAmount;
                } else {
                    inventory.Remove(item);
                }
            }
            foreach (ItemStack item in Output) {
                inventory.Add(item);
            }
        }
    }

    public static List<CraftingRecipe> ReadXML() {
        return ReadXML(GameController.Config.DEFAULT_CRAFTING_RECIPES_XML_PLACEMENT);
    }

    public static List<CraftingRecipe> ReadXML(string xmlPath) {

        string xmlValue = "";

        using (var file = new StreamReader(xmlPath)) {
            xmlValue = file.ReadToEnd();
            file.Close();
        }


        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xmlValue);

        XmlNodeList nodeListItem = doc.DocumentElement.SelectNodes("/Recipes/Recipe");

        List<CraftingRecipe> readRecipes = new List<CraftingRecipe>();

        foreach (XmlNode node in nodeListItem) {

            List<ItemStack> input = new List<ItemStack>();
            XmlNodeList recipeInput = node.SelectNodes("/Input");
            foreach (XmlNode itemInput in recipeInput) {

                Item that = GameController.GetItem(itemInput.InnerText);
                int amount = int.Parse(itemInput.Attributes["Amount"].Value);
                if (that != null) {
                    input.Add(new ItemStack(that, amount));
                }
            }

            List<ItemStack> output = new List<ItemStack>();
            XmlNodeList recipeOutput = node.SelectNodes("/Output");
            foreach (XmlNode itemOutput in recipeInput) {
                Item that = GameController.GetItem(itemOutput.InnerText);
                int amount = int.Parse(itemOutput.Attributes["Amount"].Value);
                if (that != null) {
                    output.Add(new ItemStack(that, amount));
                }
            }

            readRecipes.Add(new CraftingRecipe(input, output));
            

        }

        return readRecipes;
    }


    public override string ToString() {
        StringBuilder sb = new StringBuilder("Input: ");

        foreach (var item in Input) {
            sb.Append(item.item + " (" + item.itemAmount + ") ");
        }
        sb.Append("Output: ");

        foreach (var item in Output) {
            sb.Append(item.item + " (" + item.itemAmount + ") ");
        }

        return sb.ToString();
    }
}


public static class ExtHelp {

    public static ItemStack FindFromName(this List<ItemStack> itemStacks, string name) {
        foreach (var item in itemStacks) {
            if(item.item.Name == name) {
                return item;
            }
        }
        return null;
    }
}