using UnityEngine;

public class InventoryItemController : MonoBehaviour
{
    public Item item;

    public void RemoveInventoryItem()
    {
        InventoryManager.Instance.RemoveItem(item, 1);
    }

    /// <summary>
    /// When clicked tell what to do with the item based on the ItemType
    /// </summary>
    public void UseItem()
    {
        switch (item.itemType)
        {
            case Item.ItemType.Potion:
                if (PlayerLogic.Instance == null) break;
                PlayerLogic.Instance.IncreaseHealth(item.value);
                RemoveInventoryItem();
                break;
            case Item.ItemType.Water:
                InventoryManager.Instance.AddWater(item.value);
                RemoveInventoryItem();
                break;
            case Item.ItemType.Food:
                InventoryManager.Instance.AddFood(item.value);
                RemoveInventoryItem();
                break;
            case Item.ItemType.Metal:
                InventoryManager.Instance.AddMetal(item.value);
                RemoveInventoryItem();
                break;
            case Item.ItemType.Crystal:
                InventoryManager.Instance.AddEnergy(item.value);
                RemoveInventoryItem();
                break;
            case Item.ItemType.Component:
                InventoryManager.Instance.AddMetal(item.value);
                RemoveInventoryItem();
                break;
            case Item.ItemType.Fuel:
            case Item.ItemType.Weapon:
            default:
                break;
        }
    }
}