using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> items = new List<Item>();


    public Transform itemContent;
    public GameObject inventoryItem;

    public InventoryItemController[] inventoryItemsController;
    public void Update()
    {
        //ListItems();
    }

    private void Awake()
    {
       
       Instance = this;
        
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public void RemoveDuplicate()
    {
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }

    }

    public void ListItems()
    {
        RemoveDuplicate();

        foreach (var item in items)
        {
            GameObject newItem = Instantiate(inventoryItem, itemContent);
            var ItemName = newItem.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();
            var ItemIcon = newItem.transform.Find("Image").GetComponent<UnityEngine.UI.Image>();

            ItemName.text = item.itemName;
            ItemIcon.sprite = item.icon;
        }
        //Debug.Log("WTF");
        SetInventoryItems();
    }

 
    public void SetInventoryItems()
    {

        inventoryItemsController = itemContent.GetComponentsInChildren<InventoryItemController>();

        for(int i = 0; i<items.Count; i++)
        {
            inventoryItemsController[i].AddItem(items[i]);
        }
    }
}
