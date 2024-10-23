using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemController : MonoBehaviour
{
    public Item item;

    public void RemoveInventoryItem()
    {
        InventoryManager.Instance.RemoveItem(item);
        Destroy(gameObject);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public void UseItem()
    {
        

        switch (item.itemType)
        {
            case Item.ItemType.Potion:
                PlayerLogic.Instance.IncreaseHealth(item.value);
                break;
            case Item.ItemType.Fuel:
                ShipHealth.Instance.IncreaseFuel(item.value);
                break;
            default:
                Debug.Log("No more item types");
                break;

        }

        RemoveInventoryItem();

    }



}
