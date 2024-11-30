using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] int maxHealth;
    [SerializeField] GameObject Player;
    public GameObject DropItem;
    public List<Item> lootTable;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (Player == null) Player = GameObject.FindWithTag("Player");
        health = maxHealth;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void damage(int damage)
    {
        if (health >= 0)
        {
            health -= damage;
            if (spriteRenderer != null) StartCoroutine(ShowDamage());
        }

        if (health <= 0)
        { 
            InstantiateLoot(transform.position);
            Destroy(gameObject);
        }
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

    Item GetDroppedItem()
    {
        int randomNumber = Random.Range(0, lootTable.Count);

        for (int i = 0; i < lootTable.Count; i++)
        {
            if (i == randomNumber)
            {
                return lootTable[i];
            }
        }

        return null;
    }

    private IEnumerator ShowDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(1);
        spriteRenderer.color = Color.white;
    }
}