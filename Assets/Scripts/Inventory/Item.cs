using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


[CreateAssetMenu(fileName = "Items", menuName = "Item/Create New Item")]

public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public int value;
    public Sprite icon;
    public bool craftable;
    public ItemType itemType;

    public enum ItemType
    {
        Resource,
        Food,
        Fuel,
        Potion
    }

}


