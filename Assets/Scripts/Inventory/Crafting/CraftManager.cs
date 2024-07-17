using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    
    public List<CreateCraft> crafts = new List<CreateCraft>();
    public List<Item> itemToCraft = new List<Item>();


    public Transform craftContent;
    public GameObject craft;

    private bool canCraft = false;

    public void checkItems()
    {
        // Get the inventory instance
        InventoryManager inventoryManager = InventoryManager.Instance;

        // Iterate through all crafts
        foreach (var craftItem in crafts)
        {
            // Check if required items are present in the inventory
            bool hasItemNeeded1 = inventoryManager.items.Exists(item => item.itemName == craftItem.item1.itemName);
            bool hasItemNeeded2 = inventoryManager.items.Exists(item => item.itemName == craftItem.item2.itemName);

            if (hasItemNeeded1 && hasItemNeeded2)
            {
                Debug.Log($"You have the items needed to craft {craftItem.craftName}");
                canCraft = true;
            }
            else
            {
                Debug.Log($"You do not have the items needed to craft {craftItem.craftName}");
                canCraft = false;
            }
        }

    }

    public void craftItem()
    {
          // Get the inventory instance
        InventoryManager inventoryManager = InventoryManager.Instance;

        // Iterate through all crafts
        foreach (var craftItem in crafts)
        {
            if (canCraft)
            {
                // Remove the required items from the inventory
                inventoryManager.RemoveItem(craftItem.item1);
                inventoryManager.RemoveItem(craftItem.item2);


                // Add the crafted item to the inventory
                foreach (var item in itemToCraft)
                {

                    if (item.itemName == craftItem.craftName)
                        inventoryManager.AddItem(item);
                }

            }
            else
            {
                Debug.Log("You need to have the required items to craft this item");
            }
        
        }
    }


   
    public void ListItems()
    {
        foreach (Transform item in craftContent)
        {
            Destroy(item.gameObject);
        }


        foreach (var item in crafts)
        {
            GameObject newItem = Instantiate(craft, craftContent);
            var ItemName = newItem.transform.Find("CraftName").GetComponent<TMPro.TextMeshProUGUI>();
            var ItemIcon = newItem.transform.Find("Image").GetComponent<UnityEngine.UI.Image>();
            var ItemNeeded1 = newItem.transform.Find("ItemNeeded1").GetComponent<TMPro.TextMeshProUGUI>();
            var ItemNeeded2 = newItem.transform.Find("ItemNeeded2").GetComponent<TMPro.TextMeshProUGUI>();
            //var Item1 = newItem.transform.Find("Item1").GetComponent<TMPro.TextMeshProUGUI>();

            ItemName.text = item.craftName;
            ItemIcon.sprite = item.icon;
            ItemNeeded1.text = item.item1.itemName;
            ItemNeeded2.text = item.item2.itemName;
            //Item1.text = item.item1.itemName;
        }
    }
}
