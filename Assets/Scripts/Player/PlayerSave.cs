using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GetComponent<InventoryManager>();
    }
}