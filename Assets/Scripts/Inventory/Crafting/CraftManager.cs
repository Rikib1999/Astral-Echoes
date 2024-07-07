using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    
    public List<CreateCraft> crafts = new List<CreateCraft>();


    public Transform craftContent;
    public GameObject craft;


   
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

            ItemName.text = item.craftName;
            ItemIcon.sprite = item.icon;
        }
    }
}
