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
            ItemNeeded1.text = item.item1.itemName;
            ItemNeeded2.text = item.item2 != null ? item.item2.itemName : "";
            ItemNeeded3.text = item.item3 != null ? item.item3.itemName : "";

            newItem.GetComponent<Crafting>().craftItem = item;
            newItem.GetComponent<Crafting>().outputItem = item.ItemToCraft;
        }
    }
}