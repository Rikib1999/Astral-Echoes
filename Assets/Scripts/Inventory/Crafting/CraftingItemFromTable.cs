using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingItemFromTable : MonoBehaviour
{
    public List<CreateCraft> crafts = new List<CreateCraft>();
    public List<Item> itemToCraft = new List<Item>();

    public void craftItem()
    {
        // Get the inventory instance
        InventoryManager inventoryManager = InventoryManager.Instance;

        // Iterate through all crafts
        foreach (var craftItem in crafts)
        {
            // Check if required items are present in the inventory
            bool hasItemNeeded1 = inventoryManager.items.Exists(item => item.itemName == craftItem.itemNeeded1);
            bool hasItemNeeded2 = inventoryManager.items.Exists(item => item.itemName == craftItem.itemNeeded2);

            if (hasItemNeeded1 && hasItemNeeded2)
            {
                // Remove the required items from the inventory
                inventoryManager.RemoveItem(craftItem.item1);
                inventoryManager.RemoveItem(craftItem.item2);

                Debug.Log("Jsem tu");

                // Add the crafted item to the inventory
                foreach (var item in itemToCraft)
                {
                    Debug.Log("Jsem v ¨foreach");

                    if (item.itemName == craftItem.craftName)
                        Debug.Log("Jsem v IF");
                    inventoryManager.AddItem(item);
                }

            }
        }
    }
}
