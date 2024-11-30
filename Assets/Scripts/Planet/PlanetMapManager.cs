using Assets.Scripts.SpaceSystem;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class PlanetMapManager : NetworkSingleton<PlanetMapManager>
    {
        [SerializeField] private UnityEditor.SceneAsset planet_scene;
        [SerializeField] public NetworkVariable<SpaceObjectDataBag> PlanetDataBag = new();

        private SpaceObjectDataBag centralObjectStorage;
        private List<SpaceObjectDataBag> satelliteObjectsStorage = new();

        public static int Seed { get; private set; }

        public void LandPlanet(SpaceObjectDataBag planetDataBag)
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            centralObjectStorage = SystemMapManager.Instance.CentralObject.Value;
            foreach (var item in SystemMapManager.Instance.SatelliteObjects)
            {
                satelliteObjectsStorage.Add(item);
            }

            PlanetDataBag.Value = planetDataBag;
            SystemMapManager.Instance.SatelliteObjects.Clear();
            SystemMapManager.Instance.CentralObject.Value = default;

            if (IsServer)
            {
                LoadPlanetSceneClientRpc();
                LoadPlanetScene();
            }
        }

        private void OnSceneUnloaded(Scene current)
        {
            if (current.name != planet_scene.name) return;

            SystemMapManager.Instance.CentralObject.Value = centralObjectStorage;

            foreach (var item in satelliteObjectsStorage)
            {
                SystemMapManager.Instance.SatelliteObjects.Add(item);
            }
        }

        [ClientRpc]
        private void LoadPlanetSceneClientRpc()
        {
            ComputeSeed();
        }

        private void LoadPlanetScene()
        {
            ComputeSeed();
            
            var status = NetworkManager.SceneManager.LoadScene(planet_scene.name,LoadSceneMode.Single);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load planet scene with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }

        public void ComputeSeed()
        {
            Seed = (int)((PlanetDataBag.Value.Coordinates.x + PlanetDataBag.Value.Coordinates.y) * (PlanetDataBag.Value.Coordinates.x / Mathf.Max(Mathf.Abs(PlanetDataBag.Value.Coordinates.y), 7)));
        }
    }
}