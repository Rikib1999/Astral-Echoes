using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Item/Create New Item")]

public class Item : ScriptableObject
{
    // information about the items
    public int id;
    public string itemName;
    public int value;
    public Sprite icon;
    public bool craftable;
    public ItemType itemType;
    public int count;

    // types of items
    public enum ItemType
    {
        Metal,
        Food,
        Water,
        Fuel,
        Potion,
        Component,
        Crystal,
        Weapon
    }
}