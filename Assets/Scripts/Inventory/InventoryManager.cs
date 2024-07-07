using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> items = new List<Item>();


    public Transform itemContent;
    public GameObject inventoryItem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }


    public void ListItems()
    {
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }


        foreach (var item in items)
        {
            GameObject newItem = Instantiate(inventoryItem, itemContent);
            var ItemName = newItem.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();
            var ItemIcon = newItem.transform.Find("Image").GetComponent<UnityEngine.UI.Image>();

            ItemName.text = item.itemName;
            ItemIcon.sprite = item.icon;
        }
    }
}
