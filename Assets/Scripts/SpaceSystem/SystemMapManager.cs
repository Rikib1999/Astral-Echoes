using Assets.Scripts.Enums;
using Assets.Scripts.SpaceObjects;
using Assets.Scripts.SpaceSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class SystemMapManager : Singleton<SystemMapManager>
    {
        [SerializeField] private GameObject starPrefab;
        [SerializeField] private GameObject blackHolePrefab;
        [SerializeField] private GameObject planetPrefab;
        [SerializeField] private GameObject gasGiantPrefab;

        private const float scaleUpConst = 10;

        public static SystemDataBag SystemDataBag { get; set; }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (SystemDataBag == null) return;
            GenerateSystem();
        }

        public void EnterSystem(SystemDataBag systemDataBag)
        {
            SystemDataBag = systemDataBag;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("SystemMap");
        }

        private void GenerateSystem()
        {
            bool isStar = SystemDataBag.CentralObject.Type == eSpaceObjectType.Star;

            var centralObjectPrefab = isStar ? starPrefab : blackHolePrefab;
            var centralObject = Instantiate(centralObjectPrefab, Vector3.zero, Quaternion.identity);

            if (isStar)
            {
                var star = centralObject.GetComponent<Star>();

                star.SetSubType((eStarType)SystemDataBag.CentralObject.SubType);
                star.SetSize(SystemDataBag.CentralObject.Size * scaleUpConst);
                star.SetName(SystemDataBag.CentralObject.Name);
                star.SetCoordinates(SystemDataBag.CentralObject.Coordinates);
                star.SetTooltip(scaleUpConst);
                star.SetSprite();
            }
            else
            {
                var blackHole = centralObject.GetComponent<BlackHole>();

                blackHole.SetSubType((eBlackHoleType)SystemDataBag.CentralObject.SubType);
                blackHole.SetSize(SystemDataBag.CentralObject.Size * scaleUpConst);
                blackHole.SetName(SystemDataBag.CentralObject.Name);
                blackHole.SetCoordinates(SystemDataBag.CentralObject.Coordinates);
                blackHole.SetTooltip(scaleUpConst);
                blackHole.SetSprite();
            }

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
                    planet.SetName(satellite.Name);
                    planet.SetCoordinates(satellite.Coordinates);
                    planet.SetTooltip(scaleUpConst);
                    planet.SetSprite();
                }
                else
                {
                    var gasGiant = satelliteObject.GetComponent<GasGiant>();

                    gasGiant.SetOrbit(Vector2.zero, satellite.OrbitRadius * scaleUpConst);
                    gasGiant.SetSubType((eGasGiantType)satellite.SubType);
                    gasGiant.SetSize(satellite.Size * scaleUpConst);
                    gasGiant.SetName(satellite.Name);
                    gasGiant.SetCoordinates(satellite.Coordinates);
                    gasGiant.SetTooltip(scaleUpConst);
                    gasGiant.SetSprite();
                }
            }
        }
    }
}