using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class CraftManager : MonoBehaviour
{

    public List<CreateCraft> crafts = new List<CreateCraft>();
   //public List<Item> itemToCraft = new List<Item>();
    public Crafting[] craftings;
    //public Crafting[] itemsToCraft;


    public Transform craftContent;
    public GameObject craft;

    /*public void checkItems()
    {
        // Get the inventory instance
        InventoryManager inventoryManager = InventoryManager.Instance;

        // Iterate through all crafts
        foreach (var craftItem in crafts)
        {
            // Check if required items are present in the inventory
            bool hasItemNeeded1 = inventoryManager.items.Exists(item => item.id == craftItem.item1.id);
            bool hasItemNeeded2 = inventoryManager.items.Exists(item => item.id == craftItem.item2.id);

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
            // Check if required items are present in the inventory
            bool hasItemNeeded1 = inventoryManager.items.Exists(item => item.id == craftItem.item1.id);
            bool hasItemNeeded2 = inventoryManager.items.Exists(item => item.id == craftItem.item2.id);

            if (hasItemNeeded1 && hasItemNeeded2)
            {
                // Remove the required items from the inventory
                inventoryManager.RemoveItem(craftItem.item1);
                inventoryManager.RemoveItem(craftItem.item2);

            }
        }
    }

   
    */
   
    public void RemoveDuplicateCraft()
    {
        foreach (Transform item in craftContent)
        {
            Destroy(item.gameObject);
        }

    }

    public void ListItems()
    {
     
        RemoveDuplicateCraft();

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
        SetCraftsItems();
    }
    public void SetCraftsItems()
    {

        craftings = craftContent.GetComponentsInChildren<Crafting>();

        for (int i = 0; i < crafts.Count; i++)
        {

            craftings[i].AddCraft(crafts[i]);
        }
    }

}
