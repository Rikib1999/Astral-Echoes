using Assets.Scripts;
using Assets.Scripts.Resources;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public Transform itemContent;
    public GameObject inventoryItemPrefab;
    public Item[] possibleItems;
    public ResourceTextUpdater resourceTextUpdater;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        foreach (var item in possibleItems)
        {
            item.count = PlayerPrefs.GetInt(item.itemName, 0);
        }

        ListItems();
    }

    public void AddWater(float amount)
    {
        resourceTextUpdater.SaveWater();
        float water = PlayerPrefs.GetFloat("water", ResourceDefaultValues.Water) + amount;
        resourceTextUpdater.SetWater(water);
        PlayerPrefs.SetFloat("water", water);

        ScoreManager.AddResource((int)amount);
    }

    public void AddFood(float amount)
    {
        resourceTextUpdater.SaveFood();
        float food = PlayerPrefs.GetFloat("food", ResourceDefaultValues.Food) + amount;
        resourceTextUpdater.SetFood(food);
        PlayerPrefs.SetFloat("food", food);

        ScoreManager.AddResource((int)amount);
    }

    public void AddMetal(float amount)
    {
        resourceTextUpdater.SaveMetal();
        float metal = PlayerPrefs.GetFloat("metal", ResourceDefaultValues.Metal) + amount;
        resourceTextUpdater.SetMetal(metal);
        PlayerPrefs.SetFloat("metal", metal);

        ScoreManager.AddResource((int)amount);
    }

    public void AddEnergy(float amount)
    {
        resourceTextUpdater.SaveEnergy();
        float energy = PlayerPrefs.GetFloat("energy", ResourceDefaultValues.Energy) + amount;
        resourceTextUpdater.SetEnergy(energy);
        PlayerPrefs.SetFloat("energy", energy);

        ScoreManager.AddResource((int)amount);
    }

    /// <summary>
    /// Add item to the inventory list
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item)
    {
        var inventoryItem = possibleItems.First(x => x.id == item.id);

        if (inventoryItem.count < 0) inventoryItem.count = 0;
        inventoryItem.count++;

        PlayerPrefs.SetInt(item.itemName, inventoryItem.count);
        
        ListItems();
    }
    /// <summary>
    /// Remove item from the inventory list
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public void RemoveItem(Item item, int count)
    {
        var inventoryItem = possibleItems.First(x => x.id == item.id);

        inventoryItem.count -= count;
        if (inventoryItem.count < 0) inventoryItem.count = 0;

        PlayerPrefs.SetInt(item.itemName, inventoryItem.count);

        ListItems();
    }
    /// <summary>
    /// Delete duplicate items from the inventory list
    /// </summary>
    public void ClearList()
    {
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }
    }

    /// <summary>
    /// Will list all the items that were picked up from the ground and are in the inventory list
    /// </summary>
    public void ListItems()
    {
        ClearList();

        foreach (var item in possibleItems)
        {
            if (item.count <= 0) continue;

            GameObject newItem = Instantiate(inventoryItemPrefab, itemContent);
            var ItemName = newItem.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var ItemIcon = newItem.transform.Find("Image").GetComponent<UnityEngine.UI.Image>();
            var ItemCount = newItem.transform.Find("ItemCount").GetComponent<TextMeshProUGUI>();

            ItemName.text = item.itemName;
            ItemIcon.sprite = item.icon;
            ItemCount.text = item.count + "x";

            newItem.GetComponent<InventoryItemController>().item = item;
        }
    }
}