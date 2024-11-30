using Assets.Scripts.Resources;
using Assets.Scripts.SpaceSystem;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Player
{
    public class PlayerDeath : NetworkBehaviour
    {
        [SerializeField] public UnityEditor.SceneAsset return_scene;

        public void Die()
        {
            PlayerPrefs.SetFloat("water", ResourceDefaultValues.Water);
            PlayerPrefs.SetFloat("food", ResourceDefaultValues.Food);
            PlayerPrefs.SetFloat("energy", ResourceDefaultValues.Energy);
            PlayerPrefs.SetFloat("fuel", ResourceDefaultValues.Fuel);
            PlayerPrefs.SetFloat("maxFuel", ResourceDefaultValues.MaxFuel);
            PlayerPrefs.SetFloat("metal", ResourceDefaultValues.Metal);
            PlayerPrefs.SetFloat("currentSystemPositionX", 0);
            PlayerPrefs.SetFloat("currentSystemPositionY", 0);

            SystemMapManager.Instance.CentralObject = new NetworkVariable<SpaceObjectDataBag> { Value = new SpaceObjectDataBag() { Type = 0 } };
            var status = NetworkManager.SceneManager.LoadScene(return_scene.name, LoadSceneMode.Single);
        }
    }
}