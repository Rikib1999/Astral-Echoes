using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{

    InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < inventoryManager.items.Count; i++)
        {
            var item = inventoryManager.items[i];

            PlayerPrefs.SetInt("ID#"+i, item.id);
            PlayerPrefs.SetString("itemName#" + i, item.itemName);
            PlayerPrefs.SetInt("Value#"+i, item.value);
            PlayerPrefs.SetString("Icon#" + i, item.icon.ToString());
            PlayerPrefs.SetInt("Craftable#" + i, item.craftable ? 1 : 0);
            PlayerPrefs.SetString("ItemTime#" + i, item.itemType.ToString());

            /*
            public int id;
            public string itemName;
            public int value;
            public Sprite icon;
            public bool craftable;
            public ItemType itemType;
             */
            Debug.Log(item.icon.ToString());
        }


    }

}
