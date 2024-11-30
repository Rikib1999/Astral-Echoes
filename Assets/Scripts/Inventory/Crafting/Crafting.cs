using System.Linq;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public CreateCraft craftItem; 
    public Item outputItem;
    InventoryManager inventoryManager = InventoryManager.Instance;

    public void CraftIt()
    {
        bool hasItemNeeded1 = inventoryManager.possibleItems.First(item => item.id == craftItem.item1.id).count > 0;
        bool hasItemNeeded2 = inventoryManager.possibleItems.First(item => item.id == craftItem.item2.id).count > 0 || craftItem.item2.id == 0;
        bool hasItemNeeded3 = inventoryManager.possibleItems.First(item => item.id == craftItem.item3.id).count > 0 || craftItem.item3.id == 0;

        if (hasItemNeeded1 && hasItemNeeded2 && hasItemNeeded3)
        {
            // Remove the required items from the inventory
            inventoryManager.RemoveItem(craftItem.item1, craftItem.itemNeeded1);
            if (craftItem.item2.id != 0) inventoryManager.RemoveItem(craftItem.item2, craftItem.itemNeeded2);
            if (craftItem.item3.id != 0) inventoryManager.RemoveItem(craftItem.item3, craftItem.itemNeeded3);

            inventoryManager.AddItem(outputItem);
        }
    }
}