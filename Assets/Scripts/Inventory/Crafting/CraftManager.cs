using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    public List<CreateCraft> crafts = new List<CreateCraft>();
    public Crafting[] craftings;
    public Transform craftContent;
    public GameObject craft;

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

            ItemName.text = item.craftName;
            ItemIcon.sprite = item.icon;
            ItemNeeded1.text = item.item1.itemName;
            ItemNeeded2.text = item.item2.itemName;
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