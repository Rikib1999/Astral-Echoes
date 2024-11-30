using UnityEngine;

[CreateAssetMenu(fileName = "Crafts", menuName = "Item/Create New Crafting")]

public class CreateCraft : ScriptableObject
{
    public int id;
    public int itemID;
    public string craftName;
    public Sprite icon;
    public int itemNeeded1;
    public int itemNeeded2;
    public int itemNeeded3;
    public Item item1;
    public Item item2;
    public Item item3;
    public Item ItemToCraft;
}