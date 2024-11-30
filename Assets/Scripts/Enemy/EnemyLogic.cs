using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] int maxHealth;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject DropItem;
    public List<GameObject> lootTable;
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

            //Random drop from enemy, from the list of items
            var rand = Random.Range(0, lootTable.Count);

            for(int i = 0; i < lootTable.Count; i++)
            {
                if (i == rand)
                {
                    Instantiate(lootTable[i], transform.position, transform.rotation);
                }
            }
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