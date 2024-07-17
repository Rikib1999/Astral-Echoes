using UnityEngine;


[CreateAssetMenu(fileName = "Crafts", menuName = "Item/Create New Crafting")]

public class CreateCraft : ScriptableObject
{
    public int id;
    public string craftName;
    public Sprite icon;
    public string itemNeeded1;
    public string itemNeeded2;
    public Item item1;
    public Item item2;
}
