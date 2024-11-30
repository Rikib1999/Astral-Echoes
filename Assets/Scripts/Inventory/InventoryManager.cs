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
    }

    public void AddFood(float amount)
    {
        resourceTextUpdater.SaveFood();
        float food = PlayerPrefs.GetFloat("food", ResourceDefaultValues.Food) + amount;
        resourceTextUpdater.SetFood(food);
        PlayerPrefs.SetFloat("food", food);
    }

    public void AddMetal(float amount)
    {
        resourceTextUpdater.SaveMetal();
        float metal = PlayerPrefs.GetFloat("metal", ResourceDefaultValues.Metal) + amount;
        resourceTextUpdater.SetMetal(metal);
        PlayerPrefs.SetFloat("metal", metal);
    }

    public void AddEnergy(float amount)
    {
        resourceTextUpdater.SaveEnergy();
        float energy = PlayerPrefs.GetFloat("energy", ResourceDefaultValues.Energy) + amount;
        resourceTextUpdater.SetEnergy(energy);
        PlayerPrefs.SetFloat("energy", energy);
    }

    public void AddItem(Item item)
    {
        var inventoryItem = possibleItems.First(x => x.id == item.id);

        inventoryItem.count++;

        PlayerPrefs.SetInt(item.itemName, inventoryItem.count);
        
        ListItems();
    }

    public void RemoveItem(Item item, int count)
    {
        var inventoryItem = possibleItems.First(x => x.id == item.id);

        inventoryItem.count -= count;

        PlayerPrefs.SetInt(item.itemName, inventoryItem.count);

        ListItems();
    }

    public void ClearList()
    {
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }
    }

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