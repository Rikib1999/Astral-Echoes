using System.Linq;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public CreateCraft craftItem; 
    public Item item;
    InventoryManager inventoryManager = InventoryManager.Instance;

    public void AddCraft(CreateCraft newCraft)
    {
        craftItem = newCraft;
        AddItemToCraft(craftItem.ItemToCraft);
    }

    public void AddItemToCraft(Item newItem)
    {
        item = newItem;
    }

    public void CraftIt()
    {
        bool hasItemNeeded1 = inventoryManager.possibleItems.First(item => item.id == craftItem.item1.id).count > 0;
        bool hasItemNeeded2 = inventoryManager.possibleItems.First(item => item.id == craftItem.item2.id).count > 0;

        if (hasItemNeeded1 && hasItemNeeded2)
        {
            // Remove the required items from the inventory
            inventoryManager.RemoveItem(craftItem.item1);
            inventoryManager.RemoveItem(craftItem.item2);

            inventoryManager.AddItem(item);

            Debug.Log(item.name);
            Debug.Log(craftItem.name);
        }
        else
        {
            Debug.Log("You do not have the items");
        }
    }
}