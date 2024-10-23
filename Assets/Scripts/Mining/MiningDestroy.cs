using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MiningDestryo : MonoBehaviour
{
    public int health = 3;
    public GameObject item;
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
            Instantiate(item, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogWarning("Capsule prefab is not assigned.");
        }
    }

}