using Assets.Scripts.Enums;
using Assets.Scripts.SpaceObjects;
using Assets.Scripts.SpaceSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace Assets.Scripts
{
    public class SystemMapManager : NetworkSingleton<SystemMapManager>
    {
        [SerializeField] private NetworkObject starPrefab;
        [SerializeField] private NetworkObject blackHolePrefab;
        [SerializeField] private NetworkObject planetPrefab;
        [SerializeField] private NetworkObject gasGiantPrefab;

        [SerializeField] private EnemySpawner enemySpawner;

        [SerializeField] private UnityEditor.SceneAsset system_map_scene;

        //[SerializeField] public static SystemDataBag SystemDataBag;

        public const float scaleUpConst = 10;


        protected virtual void OnDestroy()
        {
            if (Instance == this)// && IsServer)
            {
                //NetworkManager.Singleton.SceneManager.OnSceneEvent -= OnSceneEvent;
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
           // base.OnDestroy();
        }
        void Start()
        {
            if(!IsServer)
            {
                enabled=false;
            }
        }

        private void OnSceneEvent(SceneEvent sceneEvent)
        {
            /*if(sceneEvent.SceneEventType != SceneEventType.LoadEventCompleted)
            {
                return;
            }*/
            if (Instance.gameObject.GetComponent<SystemDataBag>() == null) return;
            GenerateSystem();
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (Instance.gameObject.GetComponent<SystemDataBag>() == null) return;
            Debug.Log(JsonUtility.ToJson(Instance.gameObject.GetComponent<SystemDataBag>(), true));
            GenerateSystem();
        }

        public void EnterSystem(SystemDataBag systemDataBag)
        {
            Instance.gameObject.AddComponent<SystemDataBag>();
            Instance.gameObject.GetComponent<NetworkObject>().Spawn(true);
            var bag = Instance.gameObject.GetComponent<SystemDataBag>();
            bag.CentralObject.Value = systemDataBag.CentralObject.Value;
            foreach(SpaceObjectDataBag satObject in systemDataBag.SatelliteObjects)
            {
                bag.SatelliteObjects.Add(satObject);
            }
            //NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;
            SceneManager.sceneLoaded += OnSceneLoaded;

            Debug.Log(JsonUtility.ToJson(bag, true));

            //SceneManager.LoadScene("SystemMap");
            var status = NetworkManager.SceneManager.LoadScene(system_map_scene.name,LoadSceneMode.Single);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load system map scene with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }

        private void GenerateSystem()
        {
            var SystemDataBag = Instance.gameObject.GetComponent<SystemDataBag>();
            bool isStar = SystemDataBag.CentralObject.Value.Type == eSpaceObjectType.Star;

            var centralObjectPrefab = isStar ? starPrefab : blackHolePrefab;
            var centralObject = Instantiate(centralObjectPrefab,Vector3.zero, Quaternion.identity);

            if (isStar)
            {
                var star = centralObject.GetComponent<Star>();

                star.SetSubType((eStarType)SystemDataBag.CentralObject.Value.SubType);
                star.SetSize(SystemDataBag.CentralObject.Value.Size * scaleUpConst);
                star.SetName(SystemDataBag.CentralObject.Value.Name.ToString());
                star.SetCoordinates(SystemDataBag.CentralObject.Value.Coordinates);
                star.SetTooltip(scaleUpConst);
                star.SetSprite();
            }
            else
            {
                var blackHole = centralObject.GetComponent<BlackHole>();

                blackHole.SetSubType((eBlackHoleType)SystemDataBag.CentralObject.Value.SubType);
                blackHole.SetSize(SystemDataBag.CentralObject.Value.Size * scaleUpConst);
                blackHole.SetName(SystemDataBag.CentralObject.Value.Name.ToString());
                blackHole.SetCoordinates(SystemDataBag.CentralObject.Value.Coordinates);
                blackHole.SetTooltip(scaleUpConst);
                blackHole.SetSprite();
            }
            Debug.Log("T2"+SystemDataBag.CentralObject.Value.Name.ToString()+" "+SystemDataBag.CentralObject.Value.Type);

            centralObject.GetComponent<NetworkObject>().Spawn(true);
            

            foreach (var satellite in SystemDataBag.SatelliteObjects)
            {
                bool isPlanet = satellite.Type == eSpaceObjectType.Planet;

                var satelliteObjectPrefab = isPlanet ? planetPrefab : gasGiantPrefab;
                var satelliteObject = Instantiate(satelliteObjectPrefab, new Vector3(satellite.RelativePosition.x * scaleUpConst, satellite.RelativePosition.y * scaleUpConst, 0), Quaternion.identity);

                if (isPlanet)
                {
                    var planet = satelliteObject.GetComponent<SpaceObjects.Planet>();

                    planet.SetOrbit(Vector2.zero, satellite.OrbitRadius * scaleUpConst);
                    planet.SetSubType((ePlanetType)satellite.SubType);
                    planet.SetSize(satellite.Size * scaleUpConst);
                    planet.SetName(satellite.Name.ToString());
                    planet.SetCoordinates(satellite.Coordinates);
                    planet.SetIsLandable();
                    planet.SetTooltip(scaleUpConst);
                    planet.SetSprite();
                    if (planet.IsLandable)
                    {
                        planet.AddComponent<PlanetClick>();
                        planet.GetComponent<PlanetClick>().spaceObjectDataBag = satellite;
                    }
                }
                else
                {
                    var gasGiant = satelliteObject.GetComponent<GasGiant>();

                    gasGiant.SetOrbit(Vector2.zero, satellite.OrbitRadius * scaleUpConst);
                    gasGiant.SetSubType((eGasGiantType)satellite.SubType);
                    gasGiant.SetSize(satellite.Size * scaleUpConst);
                    gasGiant.SetName(satellite.Name.ToString());
                    gasGiant.SetCoordinates(satellite.Coordinates);
                    gasGiant.SetTooltip(scaleUpConst);
                    gasGiant.SetSprite();
                }
                Debug.Log("T2"+satellite.Name.ToString()+" "+satellite.Type);

                satelliteObject.GetComponent<NetworkObject>().Spawn(true);
            }
        }
    }
}