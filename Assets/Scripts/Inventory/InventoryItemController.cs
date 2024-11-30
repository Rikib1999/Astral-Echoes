using UnityEngine;

public class InventoryItemController : MonoBehaviour
{
    public Item item;

    public void RemoveInventoryItem()
    {
        InventoryManager.Instance.RemoveItem(item);
    }

    public void UseItem()
    {
        switch (item.itemType)
        {
            case Item.ItemType.Potion:
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
            case Item.ItemType.Fuel:
            case Item.ItemType.Weapon:
            default:
                break;
        }
    }
}