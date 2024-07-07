using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Crafts", menuName = "Item/Create New Crafting")]

public class CreateCraft : ScriptableObject
{
    public int id;
    public string craftName;
    public Sprite icon;
}
