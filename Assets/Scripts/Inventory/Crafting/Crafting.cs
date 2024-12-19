using Assets.Scripts.Resources;
using System.Linq;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public CreateCraft craftItem; 
    public Item outputItem;
    InventoryManager inventoryManager = InventoryManager.Instance;

    public void CraftIt()
    {
        switch (craftItem.craftType)
        {
            case eCraft.Ammo:

                float metals = PlayerPrefs.GetFloat("metal", ResourceDefaultValues.Metal);
                if (metals < craftItem.itemNeeded1) break;

                PlayerPrefs.SetFloat("metal", metals - craftItem.itemNeeded1);
                inventoryManager.resourceTextUpdater.SetMetal(metals - craftItem.itemNeeded1);

                //add ammo
                int ammo = PlayerPrefs.GetInt("ammo", ResourceDefaultValues.Ammo);
                ammo += 50;

                PlayerPrefs.SetInt("ammo", ammo);
                inventoryManager.resourceTextUpdater.SetAmmo(ammo);

                break;

            case eCraft.Fuel:

                float metal = PlayerPrefs.GetFloat("metal", ResourceDefaultValues.Metal);
                if (metal < craftItem.itemNeeded1) break;
                float energy = PlayerPrefs.GetFloat("energy", ResourceDefaultValues.Energy);
                if (energy < craftItem.itemNeeded2) break;

                PlayerPrefs.SetFloat("metal", metal - craftItem.itemNeeded1);
                inventoryManager.resourceTextUpdater.SetMetal(metal - craftItem.itemNeeded1);
                PlayerPrefs.SetFloat("energy", energy - craftItem.itemNeeded2);
                inventoryManager.resourceTextUpdater.SetEnergy(energy - craftItem.itemNeeded2);

                //add fuel
                float fuel = PlayerPrefs.GetFloat("fuel", ResourceDefaultValues.Fuel);
                float maxFuel = PlayerPrefs.GetFloat("maxFuel", ResourceDefaultValues.MaxFuel);

                fuel = Mathf.Min(fuel + 300, maxFuel);
                PlayerPrefs.SetFloat("fuel", fuel);
                inventoryManager.resourceTextUpdater.SetFuel(fuel);

                break;

            case eCraft.Potion:
            case eCraft.Rifle:
            case eCraft.Shotgun:

                bool hasItemNeeded1 = inventoryManager.possibleItems.First(item => item.id == craftItem.item1.id).count > 0;
                bool hasItemNeeded2 = craftItem.item2 == null || inventoryManager.possibleItems.First(item => item.id == craftItem.item2.id).count > 0;
                bool hasItemNeeded3 = craftItem.item3 == null || inventoryManager.possibleItems.First(item => item.id == craftItem.item3.id).count > 0;

                if (hasItemNeeded1 && hasItemNeeded2 && hasItemNeeded3)
                {
                    // Remove the required items from the inventory
                    inventoryManager.RemoveItem(craftItem.item1, craftItem.itemNeeded1);
                    if (craftItem.item2 != null) inventoryManager.RemoveItem(craftItem.item2, craftItem.itemNeeded2);
                    if (craftItem.item3 != null) inventoryManager.RemoveItem(craftItem.item3, craftItem.itemNeeded3);

                    if (craftItem.craftType == eCraft.Potion)
                    {
                        inventoryManager.AddItem(outputItem);
                        break;
                    }

                    //unlock weapon rifle or shotgun
                    if (craftItem.craftType == eCraft.Rifle)
                    {
                        PlayerPrefs.SetInt("rifle", 1);
                        inventoryManager.resourceTextUpdater.SetRifle(1);
                    }

                    if (craftItem.craftType == eCraft.Shotgun)
                    {
                        PlayerPrefs.SetInt("shotgun", 1);
                        inventoryManager.resourceTextUpdater.SetShotgun(1);
                    }
                }

                break;
            
            default:
                break;
        }
    }
}