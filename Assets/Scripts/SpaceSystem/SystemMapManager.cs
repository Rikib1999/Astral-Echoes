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
        [SerializeField] private GameObject starPrefab;
        [SerializeField] private GameObject blackHolePrefab;
        [SerializeField] private GameObject planetPrefab;
        [SerializeField] private GameObject gasGiantPrefab;

        [SerializeField] private EnemySpawner enemySpawner;

        [SerializeField] private UnityEditor.SceneAsset system_map_scene;

        //[SerializeField] public static SystemDataBag SystemDataBag;
        [SerializeField] public NetworkVariable<SpaceObjectDataBag> CentralObject = new NetworkVariable<SpaceObjectDataBag>();// { get => CentralObject.Value; set => CentralObject.Value = value; }
        //public SpaceObjectDataBag CentralObject { get => centralObject.Value; set => centralObject.Value = value; }
        [field: SerializeField] public NetworkList<SpaceObjectDataBag> SatelliteObjects;// { get => SatelliteObjects.Value; set => SatelliteObjects.Value = value; }
        //public NetworkList<SpaceObjectDataBag> SatelliteObjects { get => satelliteObjects.Value; set => satelliteObjects.Value = value; }
        

        public const float scaleUpConst = 10;


        public override void OnDestroy()
        {
            if (Instance == this)// && IsServer)
            {
                NetworkManager.Singleton.SceneManager.OnSceneEvent -= OnSceneEvent;
                //SceneManager.sceneLoaded -= OnSceneLoaded;
            }
           base.OnDestroy();
        }
        public void Awake(){
            //base.Awake();
            //GetComponent<NetworkObject>().Spawn();
            SatelliteObjects = new NetworkList<SpaceObjectDataBag>();
        }
        public override void Start()
        {
            base.Start();
            //GetComponent<NetworkObject>().Spawn();
            //SatelliteObjects = new NetworkList<SpaceObjectDataBag>();
            /*if(!IsServer)
            {
                enabled=false;
            }*/
        }

        private void OnSceneEvent(SceneEvent sceneEvent)
        {
            if(sceneEvent.SceneEventType != SceneEventType.LoadEventCompleted)
            {
                return;
            }
            if (CentralObject == null) return;

            GenerateSystem();
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (CentralObject == null) return;
            //Debug.Log(JsonUtility.ToJson(Instance.gameObject.GetComponent<SystemDataBag>(), true));
            GenerateSystem();
        }

        public void EnterSystem(SystemDataBag systemDataBag)
        {
            //Instance.gameObject.AddComponent<SystemDataBag>();
            //Instance.gameObject.GetComponent<NetworkObject>().Spawn(false);
            //var bag = Instance.gameObject.GetComponent<SystemDataBag>();
            //SatelliteObjects = new NetworkList<SpaceObjectDataBag>();
            CentralObject.Value = systemDataBag.CentralObject;
            foreach(SpaceObjectDataBag satObject in systemDataBag.SatelliteObjects)
            {
                SatelliteObjects.Add(satObject);
            }
            if(IsServer)
            {
                LoadSystemScene();
                LoadSystemSceneClientRpc();
            }
        }

        [ClientRpc]
        private void LoadSystemSceneClientRpc()
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;
            //SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void LoadSystemScene()
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;
            //SceneManager.sceneLoaded += OnSceneLoaded;

            //SceneManager.LoadScene("SystemMap");
            var status = NetworkManager.SceneManager.LoadScene(system_map_scene.name,LoadSceneMode.Single);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load system map scene with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }

        private void GenerateSystem()
        {
            bool isStar = CentralObject.Value.Type == eSpaceObjectType.Star;

            var centralObjectPrefab = isStar ? starPrefab : blackHolePrefab;
            var centralObject = Instantiate(centralObjectPrefab,Vector3.zero, Quaternion.identity);

            if (isStar)
            {
                var star = centralObject.GetComponent<Star>(); star.Randomize();

                star.SetSubType((eStarType)CentralObject.Value.SubType);
                star.SetSize(CentralObject.Value.Size * scaleUpConst);
                star.SetName(CentralObject.Value.Name.ToString());
                star.SetCoordinates(CentralObject.Value.Coordinates);
                star.SetTooltip(scaleUpConst);
                star.SetSprite();
            }
            else
            {
                var blackHole = centralObject.GetComponent<BlackHole>(); blackHole.Randomize();

                blackHole.SetSubType((eBlackHoleType)CentralObject.Value.SubType);
                blackHole.SetSize(CentralObject.Value.Size * scaleUpConst);
                blackHole.SetName(CentralObject.Value.Name.ToString());
                blackHole.SetCoordinates(CentralObject.Value.Coordinates);
                blackHole.SetTooltip(scaleUpConst);
                blackHole.SetSprite();
            }
            //Debug.Log("T2"+CentralObject.Value.Name.ToString()+" "+CentralObject.Value.Type);

            //centralObject.GetComponent<NetworkObject>().Spawn(true);
            

            foreach (var satellite in SatelliteObjects)
            {
                bool isPlanet = satellite.Type == eSpaceObjectType.Planet;

                var satelliteObjectPrefab = isPlanet ? planetPrefab : gasGiantPrefab;
                var satelliteObject = Instantiate(satelliteObjectPrefab, new Vector3(satellite.RelativePosition.x * scaleUpConst, satellite.RelativePosition.y * scaleUpConst, 0), Quaternion.identity);

                if (isPlanet)
                {
                    var planet = satelliteObject.GetComponent<SpaceObjects.Planet>(); planet.Randomize();

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
                    var gasGiant = satelliteObject.GetComponent<GasGiant>(); gasGiant.Randomize();

                    gasGiant.SetOrbit(Vector2.zero, satellite.OrbitRadius * scaleUpConst);
                    gasGiant.SetSubType((eGasGiantType)satellite.SubType);
                    gasGiant.SetSize(satellite.Size * scaleUpConst);
                    gasGiant.SetName(satellite.Name.ToString());
                    gasGiant.SetCoordinates(satellite.Coordinates);
                    gasGiant.SetTooltip(scaleUpConst);
                    gasGiant.SetSprite();
                }
                //Debug.Log("T2"+satellite.Name.ToString()+" "+satellite.Type);

                //satelliteObject.GetComponent<NetworkObject>().Spawn(true);
            }
        }
    }
}