using Assets.Scripts.Enums;
using Assets.Scripts.Resources;
using Assets.Scripts.SpaceObjects;
using Assets.Scripts.SpaceSystem;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class SystemMapManager : NetworkSingleton<SystemMapManager>
    {
        [SerializeField] private GameObject starPrefab;
        [SerializeField] private GameObject blackHolePrefab;
        [SerializeField] private GameObject planetPrefab;
        [SerializeField] private GameObject gasGiantPrefab;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] public NetworkVariable<SpaceObjectDataBag> CentralObject = new();
        [field: SerializeField] public NetworkList<SpaceObjectDataBag> SatelliteObjects;
        
        public const float scaleUpConst = 10;

        public bool Death { get; set; }

        public override void OnDestroy()
        {
            if (Instance == this)
            {
                NetworkManager.Singleton.SceneManager.OnSceneEvent -= OnSceneEvent;
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            }

            base.OnDestroy();
        }

        public void Awake()
        {
            SatelliteObjects = new NetworkList<SpaceObjectDataBag>();
        }

        public override void Start()
        {
            base.Start();
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }

        private void OnSceneEvent(SceneEvent sceneEvent)
        {
            if(sceneEvent.SceneEventType != SceneEventType.LoadEventCompleted)
            {
                return;
            }
            if (CentralObject == null || CentralObject.Value.Type == 0) return;

            GenerateSystem();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (CentralObject == null || CentralObject.Value.Type == 0) return;
            GenerateSystem();
        }

        public void EnterSystem(SystemDataBag systemDataBag)
        {
            if (!systemDataBag.CanTravel) return;

            CentralObject.Value = systemDataBag.CentralObject;
            foreach(SpaceObjectDataBag satObject in systemDataBag.SatelliteObjects)
            {
                SatelliteObjects.Add(satObject);
            }
            
            if(IsServer)
            {
                SyncFuelAndPosition(systemDataBag.Fuel - systemDataBag.Distance, systemDataBag.Position);
                SyncFuelAndPositionClientRpc(systemDataBag.Fuel - systemDataBag.Distance, systemDataBag.Position);
                LoadSystemScene();
                LoadSystemSceneClientRpc();
            }
        }

        [ClientRpc]
        private void LoadSystemSceneClientRpc()
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;
        }
        [ClientRpc]
        private void SyncFuelAndPositionClientRpc(float fuel, Vector3 position, ClientRpcParams clientRpcParams = default)
        {
            PlayerPrefs.SetFloat("fuel", fuel);
            PlayerPrefs.SetFloat("currentSystemPositionX", position.x);
            PlayerPrefs.SetFloat("currentSystemPositionY", position.y);
        }
        private void OnClientConnected(ulong clientId)
        {
            if(!IsServer){return;}

            //send to connected client
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[]{clientId}
                }
            };

            SyncFuelAndPositionClientRpc(PlayerPrefs.GetFloat("fuel", ResourceDefaultValues.Fuel), new Vector3(PlayerPrefs.GetFloat("currentSystemPositionX", 0), PlayerPrefs.GetFloat("currentSystemPositionY", 0), 0), clientRpcParams);
        }

        private void LoadSystemScene()
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;

            var status = NetworkManager.SceneManager.LoadScene("SystemMap" ,LoadSceneMode.Single);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load system map scene with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }
        private void SyncFuelAndPosition(float fuel, Vector3 position)
        {
            PlayerPrefs.SetFloat("fuel", fuel);
            PlayerPrefs.SetFloat("currentSystemPositionX", position.x);
            PlayerPrefs.SetFloat("currentSystemPositionY", position.y);
        }

        private void GenerateSystem()
        {
            if (SceneManager.GetActiveScene().name != "SystemMap") return;

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
            }
        }
    }
}