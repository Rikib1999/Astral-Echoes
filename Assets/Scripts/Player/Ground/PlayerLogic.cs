using Assets.Scripts.Enums;
using Assets.Scripts.PlanetResources;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public static PlayerLogic Instance;
    private int health;
    [SerializeField] int maxHealth;

    private void Start()
    {
        health = maxHealth;
    }

    public void Awake()
    {
        Instance = this;
    }

    public void IncreaseHealth(int value)
    {
        health += value;
        Debug.Log(health);
    }

    public void damage(int damage)
    {

        health -= damage;
        Debug.Log("Player Health: " + health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnParticleCollision(GameObject other)
    {
        if (other == null || other.gameObject == null) return;

        var resourceDrop = other.GetComponent<ResourceDrop>();
        
        switch (resourceDrop.resourceType)
        {
            case eResourceType.Water: Debug.Log("collected Water"); break;
            case eResourceType.Food: Debug.Log("collected Food"); break;
            case eResourceType.Energy: Debug.Log("collected Energy"); break;
            case eResourceType.Fuel: Debug.Log("collected Fuel"); break;
            case eResourceType.Metals: Debug.Log("collected Metals"); break;
            default: break;
        }
    }
}