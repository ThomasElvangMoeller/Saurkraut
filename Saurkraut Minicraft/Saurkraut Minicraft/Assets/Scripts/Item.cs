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



    public Item(string name, string description, int value, ItemRarity itemRarity, string image) {
        this.Name = name;
        this.Description = description;
        this.Value = value;
        this.Rarity = itemRarity;
        this.ImageName = image;
        this.Image = GetTexture(GameController.Config.DEFAULT_ITEM_TEXTURE_FOLDER_PLACEMENT + image);
    }

    public override string ToString() {
        return $"Name: {Name} Value: {Value} Rarity: {Rarity} {Description}";
    }

    public int CompareTo(Item other) {
        if (this.Name == other.Name) {
            return this.Value.CompareTo(other.Value);
        } else {
            return this.Name.CompareTo(other.Name);
        }
    }

    protected Sprite GetItemImageAsSprite() {
        return Sprite.Create(Image, new Rect(Vector2.zero, new Vector2(Image.width, Image.height)), new Vector2(Image.width/2, Image.height/2));
    }


    public static List<Item> ReadXML() {
        return ReadXML(GameController.Config.DEFAULT_ITEM_XML_PLACEMENT);
    }

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
        return base.ToString()+ " Damage: "+Damage;
    }
}


public enum ItemRarity { Common, Uncommon, Rare, Epic }