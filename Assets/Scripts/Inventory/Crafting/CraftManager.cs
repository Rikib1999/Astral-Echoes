using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    
    public List<CreateCraft> crafts = new List<CreateCraft>();


    public Transform craftContent;
    public GameObject craft;

    public void checkItems()
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
                Debug.Log($"You have the items needed to craft {craftItem.craftName}");
            }
            else
            {
                Debug.Log($"You do not have the items needed to craft {craftItem.craftName}");
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

            ItemName.text = item.craftName;
            ItemIcon.sprite = item.icon;
            ItemNeeded1.text = item.itemNeeded1;
            ItemNeeded2.text = item.itemNeeded2;
        }
    }
}
