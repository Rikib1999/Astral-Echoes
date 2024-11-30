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
            PlayerPrefs.DeleteAll();

            SystemMapManager.Instance.CentralObject = new NetworkVariable<SpaceObjectDataBag> { Value = new SpaceObjectDataBag() { Type = 0 } };
            SystemMapManager.Instance.Death = true;
            var status = NetworkManager.SceneManager.LoadScene(return_scene.name, LoadSceneMode.Single);
        }
    }
}