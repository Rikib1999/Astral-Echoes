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
    public int count;

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