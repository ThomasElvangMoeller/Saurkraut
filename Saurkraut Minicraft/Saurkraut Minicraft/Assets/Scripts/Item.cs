using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;

public class Item : IComparable<Item> {

    public ItemRarity Rarity { get; private set; }
    public int Value { get; set; }
    public string Name { get; }
    public string Description { get; }

    public static string DEFAULT_XML_PLACEMENT = ".\\Items.xml";

    public static Item DummyItem = new Item("Dummy Name", "Dummy Description", 0, ItemRarity.Common);

    public Item(string name, string description, int value, ItemRarity itemRarity) {
        this.Name = name;
        this.Description = description;
        this.Value = value;
        this.Rarity = itemRarity;
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


    public static List<Item> ReadXML() {
        return ReadXML(DEFAULT_XML_PLACEMENT);
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

            readItems.Add(new Item(name, desc, value, itemRarity));
        }

        XmlNodeList nodeListWeapons = doc.DocumentElement.SelectNodes("/Items/Weapon");

        foreach (XmlNode node in nodeListWeapons) {
            string desc = node.Attributes["Description"].Value;
            string name = node.Attributes["Name"].Value;
            int value = int.Parse(node.Attributes["Value"].Value);
            ItemRarity itemRarity = (ItemRarity)int.Parse(node.Attributes["ItemRarity"].Value);
            int damage = int.Parse(node.Attributes["Damage"].Value);

            readItems.Add(new Weapon(name, desc, value, itemRarity, damage));
        }


        return readItems;
    }

}


public class Weapon : Item, IComparable<Weapon> {

    public int Damage { get; }

    public Weapon(string name, string description, int value, ItemRarity itemRarity, int damage) : base(name, description, value, itemRarity) {
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