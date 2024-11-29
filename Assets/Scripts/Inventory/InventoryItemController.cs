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
            case Item.ItemType.Resource:
                Debug.Log("Not implemented yet.");
                break;
            case Item.ItemType.Food:
                Debug.Log("Not implemented yet.");
                break;
            default:
                Debug.Log("No more item types");
                break;
        }

        if(item.itemType != Item.ItemType.Resource)
        {
            RemoveInventoryItem();
        }
    }
}