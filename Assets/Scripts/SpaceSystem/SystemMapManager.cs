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
    // Manages the system map scene and handles networked interactions
    public class SystemMapManager : NetworkSingleton<SystemMapManager>
    {
        [SerializeField] private GameObject starPrefab; // Prefab for stars
        [SerializeField] private GameObject blackHolePrefab; // Prefab for black holes
        [SerializeField] private GameObject planetPrefab; // Prefab for planets
        [SerializeField] private GameObject gasGiantPrefab; // Prefab for gas giants
        [SerializeField] private EnemySpawner enemySpawner; // Enemy spawner reference
        [SerializeField] public NetworkVariable<SpaceObjectDataBag> CentralObject = new(); // The central object (e.g., star or black hole) of the system
        [field: SerializeField] public NetworkList<SpaceObjectDataBag> SatelliteObjects; // List of satellite objects (e.g., planets, gas giants)

        public const float scaleUpConst = 10; // Scaling factor for system object positions and sizes

        public bool Death { get; set; } // Tracks player death state

        // Cleanup event handlers on destruction
        public override void OnDestroy()
        {
            if (Instance == this && NetworkManager.Singleton)
            {
                NetworkManager.Singleton.SceneManager.OnSceneEvent -= OnSceneEvent; // Unsubscribe from scene events
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected; // Unsubscribe from client connection events
            }

            base.OnDestroy();
        }

        // Initialize satellite objects as a network list
        public void Awake()
        {
            SatelliteObjects = new NetworkList<SpaceObjectDataBag>();
        }

        public override void Start()
        {
            base.Start();
        }
        // Set up network callbacks
        public override void OnNetworkSpawn()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            base.OnNetworkSpawn();
        }

        // Handles actions triggered by scene events
        private void OnSceneEvent(SceneEvent sceneEvent)
        {
            if (sceneEvent.SceneEventType != SceneEventType.LoadEventCompleted)
            {
                return; // Only proceed for completed load events
            }

            if (CentralObject == null || CentralObject.Value.Type == 0) return; // Skip if no central object data

            GenerateSystem(); // Generate the system map
        }

        // Generates the system map when the scene is loaded
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (CentralObject == null || CentralObject.Value.Type == 0) return; // Skip if no central object data
            GenerateSystem();
        }

        // Enters a system and synchronizes its data across the network
        public void EnterSystem(SystemDataBag systemDataBag)
        {
            if (!systemDataBag.CanTravel) return; // Ensure the system is travelable

            CentralObject.Value = systemDataBag.CentralObject; // Set the central object
            foreach (SpaceObjectDataBag satObject in systemDataBag.SatelliteObjects)
            {
                SatelliteObjects.Add(satObject); // Add satellite objects
            }

            if (IsServer)
            {
                // Sync fuel and position and load the system scene on the server
                SyncFuelAndPosition(systemDataBag.Fuel - systemDataBag.Distance, systemDataBag.Position);
                SyncFuelAndPositionClientRpc(systemDataBag.Fuel - systemDataBag.Distance, systemDataBag.Position, PlayerPrefs.GetInt("seed", 0));
                LoadSystemScene();
                LoadSystemSceneClientRpc();
            }
        }

        // Client RPC to handle loading the system scene
        [ClientRpc]
        private void LoadSystemSceneClientRpc()
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;
        }

        // Client RPC to synchronize fuel and position with the client
        [ClientRpc]
        private void SyncFuelAndPositionClientRpc(float fuel, Vector3 position, int seed, ClientRpcParams clientRpcParams = default)
        {
            var mySeed = PlayerPrefs.GetInt("seed", 0);

            PlayerPrefs.SetFloat("fuel", fuel); // Update fuel in PlayerPrefs
            PlayerPrefs.SetFloat("currentSystemPositionX", position.x); // Update X-coordinate
            PlayerPrefs.SetFloat("currentSystemPositionY", position.y); // Update Y-coordinate
            PlayerPrefs.SetInt("seed", seed); // Update Seed

            ScoreManager.UpdateMaxDistance((int)Vector2.Distance(position, Vector2.zero));

            Debug.Log("C"+ mySeed + " "+seed);

            MapGenerator mg = (MapGenerator)FindAnyObjectByType(typeof(MapGenerator));
            if (mg != null)
            {
                mg.SetSeedAndPlayerPosition();
                mg.RegenerateChunks();
            }
        }

        // Callback for when a new client connects
        private void OnClientConnected(ulong clientId)
        {
            if (!IsServer) return; // Only execute on the server
            // Send fuel and position data to the newly connected client
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };

            SyncFuelAndPositionClientRpc(
                PlayerPrefs.GetFloat("fuel", ResourceDefaultValues.Fuel),
                new Vector3(
                    PlayerPrefs.GetFloat("currentSystemPositionX", 0),
                    PlayerPrefs.GetFloat("currentSystemPositionY", 0),
                    0
                ),
                PlayerPrefs.GetInt("seed", 0),
                clientRpcParams
            );

            Debug.Log("S"+PlayerPrefs.GetInt("seed", 0));
        }

        // Loads the system scene on the server
        private void LoadSystemScene()
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;

            var status = NetworkManager.SceneManager.LoadScene("SystemMap", LoadSceneMode.Single);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load system map scene with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }

        // Synchronizes fuel and position data locally
        private void SyncFuelAndPosition(float fuel, Vector3 position)
        {
            PlayerPrefs.SetFloat("fuel", fuel); // Save fuel value
            PlayerPrefs.SetFloat("currentSystemPositionX", position.x); // Save X-coordinate
            PlayerPrefs.SetFloat("currentSystemPositionY", position.y); // Save Y-coordinate

            ScoreManager.UpdateMaxDistance((int)Vector2.Distance(position, Vector2.zero));
        }

        // Generates the system map, including the central object and its satellites
        private void GenerateSystem()
        {
            if (SceneManager.GetActiveScene().name != "SystemMap") return; // Ensure the active scene is "SystemMap"

            bool isStar = CentralObject.Value.Type == eSpaceObjectType.Star; // Check if the central object is a star

            // Instantiate the central object (star or black hole)
            var centralObjectPrefab = isStar ? starPrefab : blackHolePrefab;
            var centralObject = Instantiate(centralObjectPrefab, Vector3.zero, Quaternion.identity);

            if (isStar)
            {
                // Configure star properties
                var star = centralObject.GetComponent<Star>();
                star.Randomize();
                star.SetSubType((eStarType)CentralObject.Value.SubType);
                star.SetSize(CentralObject.Value.Size * scaleUpConst);
                star.SetName(CentralObject.Value.Name.ToString());
                star.SetCoordinates(CentralObject.Value.Coordinates);
                star.SetTooltip(scaleUpConst);
                star.SetSprite();
            }
            else
            {
                // Configure black hole properties
                var blackHole = centralObject.GetComponent<BlackHole>();
                blackHole.Randomize();
                blackHole.SetSubType((eBlackHoleType)CentralObject.Value.SubType);
                blackHole.SetSize(CentralObject.Value.Size * scaleUpConst);
                blackHole.SetName(CentralObject.Value.Name.ToString());
                blackHole.SetCoordinates(CentralObject.Value.Coordinates);
                blackHole.SetTooltip(scaleUpConst);
                blackHole.SetSprite();
            }

            // Instantiate and configure all satellite objects (planets or gas giants)
            foreach (var satellite in SatelliteObjects)
            {
                bool isPlanet = satellite.Type == eSpaceObjectType.Planet;

                var satelliteObjectPrefab = isPlanet ? planetPrefab : gasGiantPrefab;
                var satelliteObject = Instantiate(
                    satelliteObjectPrefab,
                    new Vector3(satellite.RelativePosition.x * scaleUpConst, satellite.RelativePosition.y * scaleUpConst, 0),
                    Quaternion.identity
                );

                if (isPlanet)
                {
                    // Configure planet properties
                    var planet = satelliteObject.GetComponent<SpaceObjects.Planet>();
                    planet.Randomize();
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
                    // Configure gas giant properties
                    var gasGiant = satelliteObject.GetComponent<GasGiant>();
                    gasGiant.Randomize();
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