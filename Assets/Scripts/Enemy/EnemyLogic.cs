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
            Destroy(gameObject);

            InstantiateLoot(transform.position);

            /*
            //Random drop from enemy, from the list of items
            var rand = Random.Range(0, lootTable.Count+2);

            for(int i = 0; i < lootTable.Count; i++)
            {
                if (i == rand)
                {
                    Instantiate(lootTable[i], transform.position, transform.rotation);
                }
                else{
                    Debug.Log("No loot dropped");
                }
            }*/
        }
    }

    Item GetDroppedItem()
    {
        int randomNumber = Random.Range(0, lootTable.Count + 2);

        List<Item> possibleItems = new List<Item>();

        for (int i = 0; i < lootTable.Count; i++)
        {
            if (i == randomNumber)
            {
                //Instantiate(lootTable[i], transform.position, transform.rotation);
                possibleItems.Add(lootTable[i]);
                 
            }

        }

        if(possibleItems.Count > 0)
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

        if(droppedItem != null)
        {
            GameObject itemGameObject = Instantiate(DropItem, spawnPos, Quaternion.identity);

            itemGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.icon;

            itemGameObject.GetComponent<ItemPickUp>().item = droppedItem;
        }
    }

    private IEnumerator ShowDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(1);
        spriteRenderer.color = Color.white;
    }

    public void OnMouseDown()
    {
        damage(10);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.GetComponent<PlayerLogic>().damage(10);
        }
    }
}