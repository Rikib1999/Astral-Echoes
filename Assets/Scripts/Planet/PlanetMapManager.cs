using Assets.Scripts.SpaceSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace Assets.Scripts
{
    public class PlanetMapManager : NetworkSingleton<PlanetMapManager>
    {
        [SerializeField] private UnityEditor.SceneAsset planet_scene;

        //public static SpaceObjectDataBag PlanetDataBag { get; set; } 
        //[SerializeField] public static SystemDataBag SystemDataBag;
        [SerializeField] public NetworkVariable<SpaceObjectDataBag> PlanetDataBag = new NetworkVariable<SpaceObjectDataBag>();// { get => CentralObject.Value; set => CentralObject.Value = value; }
        //public SpaceObjectDataBag CentralObject { get => centralObject.Value; set => centralObject.Value = value; }
 
        public static int Seed { get; private set; }



        public void LandPlanet(SpaceObjectDataBag planetDataBag)
        {
            //Debug.Log(JsonUtility.ToJson(SystemMapManager.Instance.gameObject.GetComponent<SystemDataBag>(), true));
            
            //PlanetDataBag = planetDataBag;
            PlanetDataBag.Value = planetDataBag;
            SystemMapManager.Instance.SatelliteObjects.Clear();
            SystemMapManager.Instance.CentralObject.Value = default;
            //SystemMapManager.Instance.SatelliteObjects = null;
            //SystemMapManager.Instance.CentralObject = null;

            if(IsServer)
            {
                LoadPlanetSceneClientRpc();
                LoadPlanetScene();
            }
        }
        
        [ClientRpc]
        private void LoadPlanetSceneClientRpc()
        {
            ComputeSeed();
            
            //SceneManager.LoadScene("Planet");
            /*var status = NetworkManager.SceneManager.LoadScene(planet_scene.name,LoadSceneMode.Single);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load planet scene with a {nameof(SceneEventProgressStatus)}: {status}");
            }*/
        }
        private void LoadPlanetScene()
        {
            ComputeSeed();
            
            //SceneManager.LoadScene("Planet");
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