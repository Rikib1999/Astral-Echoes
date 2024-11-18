﻿using Assets.Scripts.SpaceSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace Assets.Scripts
{
    public class PlanetMapManager : NetworkSingleton<PlanetMapManager>
    {
        [SerializeField] private UnityEditor.SceneAsset planet_scene;

        public static SpaceObjectDataBag PlanetDataBag { get; set; }
        public static int Seed { get; private set; }

        public void LandPlanet(SpaceObjectDataBag planetDataBag)
        {
            Debug.Log(JsonUtility.ToJson(SystemMapManager.Instance.gameObject.GetComponent<SystemDataBag>(), true));
            
            //var bag = this.gameObject.GetComponent<SystemDataBag>();
            PlanetDataBag = planetDataBag;
            SystemMapManager.Instance.gameObject.GetComponent<SystemDataBag>().SatelliteObjects = null;
            SystemMapManager.Instance.gameObject.GetComponent<SystemDataBag>().CentralObject = null;
            ComputeSeed();
            
            //SceneManager.LoadScene("Planet");
            var status = NetworkManager.SceneManager.LoadScene(planet_scene.name,LoadSceneMode.Single);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load planet scene with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }

        private void ComputeSeed()
        {
            Seed = (int)((PlanetDataBag.Coordinates.x + PlanetDataBag.Coordinates.y) * (PlanetDataBag.Coordinates.x / Mathf.Max(Mathf.Abs(PlanetDataBag.Coordinates.y), 7)));
        }
    }
}