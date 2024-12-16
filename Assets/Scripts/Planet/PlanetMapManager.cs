using Assets.Scripts.SpaceSystem;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class PlanetMapManager : NetworkSingleton<PlanetMapManager>
    {
        // NetworkVariable to hold data about the current planet.
        [SerializeField] public NetworkVariable<SpaceObjectDataBag> PlanetDataBag = new();

        // Local storage for the central space object.
        private SpaceObjectDataBag centralObjectStorage;

        // Local storage for satellite space objects.
        private List<SpaceObjectDataBag> satelliteObjectsStorage = new();

        public static int Seed { get; private set; }

        // Method to transition to the planet scene, storing and clearing necessary data.
        public void LandPlanet(SpaceObjectDataBag planetDataBag)
        {
            // Registering scene unload event to restore state later.
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            // Store the current central object and satellites.
            centralObjectStorage = SystemMapManager.Instance.CentralObject.Value;
            foreach (var item in SystemMapManager.Instance.SatelliteObjects)
            {
                satelliteObjectsStorage.Add(item);
            }

            // Set the current planet data and clear the system map.
            PlanetDataBag.Value = planetDataBag;
            SystemMapManager.Instance.SatelliteObjects.Clear();
            SystemMapManager.Instance.CentralObject.Value = default;

            if (IsServer)
            {
                LoadPlanetSceneClientRpc();
                LoadPlanetScene();
            }
        }

        // Event handler to restore system map state when leaving the planet scene.
        private void OnSceneUnloaded(Scene current)
        {
            if (current.name != "Planet") return;

            SystemMapManager.Instance.CentralObject.Value = centralObjectStorage;

            foreach (var item in satelliteObjectsStorage)
            {
                SystemMapManager.Instance.SatelliteObjects.Add(item);
            }
        }

        // Client RPC to compute the seed for the planet scene on connected clients.
        [ClientRpc]
        private void LoadPlanetSceneClientRpc()
        {
            ComputeSeed();
        }

        // Method to load the planet scene on the server.
        private void LoadPlanetScene()
        {
            ComputeSeed();

            var status = NetworkManager.SceneManager.LoadScene("Planet", LoadSceneMode.Single);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load planet scene with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }

        // Computes a unique seed value based on the planet's coordinates.
        public void ComputeSeed()
        {
            Seed = (int)((PlanetDataBag.Value.Coordinates.x + PlanetDataBag.Value.Coordinates.y) * (PlanetDataBag.Value.Coordinates.x / Mathf.Max(Mathf.Abs(PlanetDataBag.Value.Coordinates.y), 7)));
        }
    }
}
