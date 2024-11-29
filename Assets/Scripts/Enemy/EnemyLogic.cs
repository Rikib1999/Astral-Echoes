using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] int maxHealth;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject DropItem;
    public List<GameObject> lootTable;

    private void Start()
    {
        if (Player == null) { Player = GameObject.FindWithTag("Player"); }
        health = maxHealth;
    }

    public void damage(int damage)
    {
        Debug.Log("HIT");
        if (health >= 0)
        {
            health -= damage;
            Debug.Log(health);
        }
        if (health <= 0) { 
            Destroy(gameObject);

            //Random drop from enemy, from the list of items
            var rand = Random.Range(0, lootTable.Count);

            for(int i = 0; i < lootTable.Count; i++)
            {
                
                if (i == rand)
                {
                    Instantiate(lootTable[i], transform.position, transform.rotation);
                }
            }

           /* if (DropItem != null)
            {
                Instantiate(DropItem, transform.position, transform.rotation);
            }
            else
            {
                Debug.LogWarning("Capsule prefab is not assigned.");
            }*/
        }
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