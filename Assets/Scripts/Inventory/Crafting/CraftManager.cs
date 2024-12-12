using UnityEngine;

public class CraftManager : MonoBehaviour
{
    public CreateCraft[] possibleCrafts;
    public Transform craftContent;
    public GameObject craftPrefab;

    public void Start()
    {
        ListItems();
    }
    /// <summary>
    /// Remove duplicates from the craft list
    /// </summary>
    public void RemoveDuplicateCraft()
    {
        foreach (Transform item in craftContent)
        {
            Destroy(item.gameObject);
        }
    }

    /// <summary>
    /// List all possible crafting items and items needed to craft
    /// </summary>
    public void ListItems()
    {
        RemoveDuplicateCraft();

        foreach (var item in possibleCrafts)
        {
            GameObject newItem = Instantiate(craftPrefab, craftContent);
            var ItemName = newItem.transform.Find("CraftName").GetComponent<TMPro.TextMeshProUGUI>();
            var ItemIcon = newItem.transform.Find("Image").GetComponent<UnityEngine.UI.Image>();
            var ItemNeeded1 = newItem.transform.Find("ItemNeeded1").GetComponent<TMPro.TextMeshProUGUI>();
            var ItemNeeded2 = newItem.transform.Find("ItemNeeded2").GetComponent<TMPro.TextMeshProUGUI>();
            var ItemNeeded3 = newItem.transform.Find("ItemNeeded3").GetComponent<TMPro.TextMeshProUGUI>();

            ItemName.text = item.craftName;
            ItemIcon.sprite = item.icon;
            ItemNeeded1.text = item.resource1 == 0 ? item.item1.itemName : item.itemNeeded1 + "x " + item.resource1.ToString();
            ItemNeeded2.text = item.resource2 == 0 ? item.item2 != null ? item.item2.itemName : "" : item.itemNeeded2 + "x " + item.resource2.ToString();
            ItemNeeded3.text = item.item3 != null ? item.item3.itemName : "";

            newItem.GetComponent<Crafting>().craftItem = item;
            newItem.GetComponent<Crafting>().outputItem = item.ItemToCraft;
        }
    }
}