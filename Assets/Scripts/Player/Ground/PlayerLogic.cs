using Assets.Scripts;
using Assets.Scripts.Enums;
using Assets.Scripts.PlanetResources;
using Assets.Scripts.Player;
using Assets.Scripts.Resources;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class PlayerLogic : MonoBehaviour
{
    public static PlayerLogic Instance;
    private int health;
    [SerializeField] int maxHealth;
    [SerializeField] Slider healthBar;

    private float water;
    private float food;
    private float energy;
    private float metal;
    private float particleValue = 1f;
    private int particleCounter = 0;
    private ResourceTextUpdater resourceTextUpdater;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        health = maxHealth;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        healthBar.maxValue = maxHealth;
        healthBar.value = health;

        water = PlayerPrefs.GetFloat("water", ResourceDefaultValues.Water);
        food = PlayerPrefs.GetFloat("food", ResourceDefaultValues.Food);
        energy = PlayerPrefs.GetFloat("energy", ResourceDefaultValues.Energy);
        metal = PlayerPrefs.GetFloat("metal", ResourceDefaultValues.Metal);

        var resourcesText = GameObject.FindGameObjectWithTag("ResourcesText");
        if (resourcesText != null) resourceTextUpdater = resourcesText.GetComponent<ResourceTextUpdater>();
    }

    public void Awake()
    {
        Instance = this;
    }

    public void IncreaseHealth(int value)
    {
        health += value;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.value = health;
    }

    public void damage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.value = health;
        StartCoroutine(ShowDamage());

        if (health <= 0) GetComponent<PlayerDeath>().Die();
    }

    private IEnumerator ShowDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(1);
        spriteRenderer.color = Color.white;
    }

    public void OnParticleCollision(GameObject other)
    {
        if (other == null || other.gameObject == null || resourceTextUpdater == null) return;

        var resourceDrop = other.GetComponent<ResourceDrop>();

        bool save = false;
        particleCounter++;
        if (particleCounter > 200)
        {
            particleCounter = 0;
            save = true;
        }

        switch (resourceDrop.resourceType)
        {
            case eResourceType.Water:
                water += particleValue;
                resourceTextUpdater.SetWater(water);
                if (save)
                {
                    int original = (int)PlayerPrefs.GetFloat("water", 0);
                    PlayerPrefs.SetFloat("water", water);

                    ScoreManager.AddResource((int)(water - original));
                }
                break;
            case eResourceType.Food:
                food += particleValue;
                resourceTextUpdater.SetFood(food);
                if (save)
                {
                    int original = (int)PlayerPrefs.GetFloat("food", 0);
                    PlayerPrefs.SetFloat("food", food);

                    ScoreManager.AddResource((int)(food - original));
                }
                break;
            case eResourceType.Energy:
                energy += particleValue;
                resourceTextUpdater.SetEnergy(energy);
                if (save)
                {
                    int original = (int)PlayerPrefs.GetFloat("energy", 0);
                    PlayerPrefs.SetFloat("energy", energy);

                    ScoreManager.AddResource((int)(energy - original));
                }
                break;
            case eResourceType.Metals:
                metal += particleValue;
                resourceTextUpdater.SetMetal(metal);
                if (save)
                {
                    int original = (int)PlayerPrefs.GetFloat("metal", 0);
                    PlayerPrefs.SetFloat("metal", metal);

                    ScoreManager.AddResource((int)(metal - original));
                }
                break;
            default: break;
        }
    }
}