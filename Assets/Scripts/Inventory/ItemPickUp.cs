using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;

    void PickUp()
    {
        InventoryManager.Instance.AddItem(item);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PickUp();
        }
    }
}