using System.Collections;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] int maxHealth;
    [SerializeField] GameObject Player;
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

        if (health <= 0) Destroy(gameObject);
    }

    private IEnumerator ShowDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(1);
        spriteRenderer.color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.GetComponent<PlayerLogic>().damage(10);
        }
    }
}