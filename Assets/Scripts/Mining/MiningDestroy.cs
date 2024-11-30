using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MiningDestryo : MonoBehaviour
{
    public int health = 3;
    public GameObject item;
    public GameObject DropItem;
    public List<Item> lootTable;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Health: " + health);
        if (health <= 0)
        {
            SpanwItem();
            Destroy(gameObject);

        }
    }
    public void OnMouseDown()
    {
        TakeDamage(1);
    }
    private void SpanwItem()
    {
        if (item != null)
        {
            //Instantiate(item, transform.position, transform.rotation);
            InstantiateLoot(transform.position);
        }
        else
        {
            Debug.LogWarning("Capsule prefab is not assigned.");
        }
    }


    Item GetDroppedItem()
    {
        int randomNumber = Random.Range(0, lootTable.Count + 1);

        List<Item> possibleItems = new List<Item>();

        for (int i = 0; i < lootTable.Count; i++)
        {
            if (i == randomNumber)
            {
                //Instantiate(lootTable[i], transform.position, transform.rotation);
                possibleItems.Add(lootTable[i]);

            }

        }

        if (possibleItems.Count > 0)
        {
            Item droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        Debug.Log("No loot dropped");
        return null;
    }

    public void InstantiateLoot(Vector2 spawnPos)
    {
        Item droppedItem = GetDroppedItem();

        if (droppedItem != null)
        {
            GameObject itemGameObject = Instantiate(DropItem, spawnPos, Quaternion.identity);

            itemGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.icon;

            itemGameObject.GetComponent<ItemPickUp>().item = droppedItem;
        }
    }

}