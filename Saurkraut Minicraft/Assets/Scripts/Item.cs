using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Item : IComparable<Item> {

    public ItemRarity Rarity { get; private set; }
    public int Value { get; set; }
    public string Name { get; }
    public string Description { get; }
    public string ImageName { get; }
    public Texture2D Image { get; }


    /// <summary>
    /// Creates an Item, the parameter image is the name of the image file .png is preferred for this
    /// </summary>
    /// <param name="name">The name of the bloody item, ya dingus</param>
    /// <param name="description"></param>
    /// <param name="value"></param>
    /// <param name="itemRarity"></param>
    /// <param name="image"></param>
    public Item(string name, string description, int value, ItemRarity itemRarity, string image) {
        this.Name = name;
        this.Description = description;
        this.Value = value;
        this.Rarity = itemRarity;
        this.ImageName = image;
        this.Image = GetTexture(GameController.Config.DEFAULT_ITEM_TEXTURE_FOLDER_PLACEMENT + image);
    }

    public override string ToString() {
        return $"Name: {Name}, Value: {Value}, Rarity: {Rarity}, {Description}";
    }

    public int CompareTo(Item other) {
        if (this.Name == other.Name) {
            return this.Value.CompareTo(other.Value);
        } else {
            return this.Name.CompareTo(other.Name);
        }
    }

    /// <summary>
    /// returns a the item image as a sprite
    /// </summary>
    /// <returns></returns>
    protected Sprite GetItemImageAsSprite() {
        return Sprite.Create(Image, new Rect(Vector2.zero, new Vector2(Image.width, Image.height)), new Vector2(.5f, .5f));
    }

    /// <summary>
    /// Reads the default item xml file placement and creates a List of CraftingRecipes using
    /// </summary>
    /// <returns></returns>
    public static List<Item> ReadXML() {
        return ReadXML(GameController.Config.DEFAULT_ITEM_XML_PLACEMENT);
    }

    /// <summary>
    /// Reads the given item xml file placement and creates a List of CraftingRecipes using
    /// </summary>
    /// <returns></returns>
    public static List<Item> ReadXML(string xmlPath) {

        string xmlValue = "";

        using (var file = new StreamReader(xmlPath)) {
            xmlValue = file.ReadToEnd();
            file.Close();
        }


        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xmlValue);

        XmlNodeList nodeListItem = doc.DocumentElement.SelectNodes("/Items/Item");

        List<Item> readItems = new List<Item>();

        foreach (XmlNode node in nodeListItem) {
            
            string desc = node.Attributes["Description"].Value;
            string name = node.Attributes["Name"].Value;
            int value = int.Parse(node.Attributes["Value"].Value);
            ItemRarity itemRarity = (ItemRarity) int.Parse(node.Attributes["ItemRarity"].Value);
            string image = node.Attributes["ImageName"].Value;

            readItems.Add(new Item(name, desc, value, itemRarity, image));
        }

        XmlNodeList nodeListWeapons = doc.DocumentElement.SelectNodes("/Items/Weapon");

        foreach (XmlNode node in nodeListWeapons) {
            string desc = node.Attributes["Description"].Value;
            string name = node.Attributes["Name"].Value;
            int value = int.Parse(node.Attributes["Value"].Value);
            ItemRarity itemRarity = (ItemRarity)int.Parse(node.Attributes["ItemRarity"].Value);
            string image = node.Attributes["ImageName"].Value;
            int damage = int.Parse(node.Attributes["Damage"].Value);

            readItems.Add(new Weapon(name, desc, value, itemRarity,image, damage));
        }


        return readItems;
    }
    /// <summary>
    /// returns a Texture2D from the given fileplacement, has no error handling
    /// </summary>
    /// <param name="imagePlace"></param>
    /// <returns></returns>
    protected Texture2D GetTexture(string imagePlace) {
        Texture2D texture = new Texture2D(16,16,TextureFormat.RGBA32, false);

        MemoryStream stream = new MemoryStream();
        int imageBufferSize = 0;

        using (var file = new FileStream(imagePlace, FileMode.OpenOrCreate)) {
            while (file.ReadByte() != -1) {
                imageBufferSize++;
            }
        }

        byte[] imageBuffer = new byte[imageBufferSize];

        using (var file = new FileStream(imagePlace, FileMode.OpenOrCreate)) {
            file.Read(imageBuffer, 0, imageBuffer.Length);
        }

        texture.LoadImage(imageBuffer);
        return texture;
    }

}

[Serializable]
public class Weapon : Item, IComparable<Weapon> {

    public int Damage { get; }

    public Weapon(string name, string description, int value, ItemRarity itemRarity,string image, int damage) : base(name, description, value, itemRarity, image) {
        this.Damage = damage;
    }

    public int CompareTo(Weapon other) {
        if (this.Damage == other.Damage) {
            return base.CompareTo(other);
        } else {
            return this.Damage.CompareTo(other.Damage);
        }
    }

    public override string ToString() {
        return base.ToString()+ ", Damage: "+Damage;
    }
}


public enum ItemRarity { Common, Uncommon, Rare, Epic }