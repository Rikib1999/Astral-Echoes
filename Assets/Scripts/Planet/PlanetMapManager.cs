using Assets.Scripts.SpaceSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class PlanetMapManager : Singleton<PlanetMapManager>
    {
        public static SpaceObjectDataBag PlanetDataBag { get; set; }
        public static int Seed { get; private set; }

        public void LandPlanet(SpaceObjectDataBag planetDataBag)
        {
            SystemMapManager.SystemDataBag = null;
            PlanetDataBag = planetDataBag;
            ComputeSeed();
            SceneManager.LoadScene("Planet");
        }

        private void ComputeSeed()
        {
            Seed = (int)((PlanetDataBag.Coordinates.x + PlanetDataBag.Coordinates.y) * (PlanetDataBag.Coordinates.x / Mathf.Max(Mathf.Abs(PlanetDataBag.Coordinates.y), 7)));
        }
    }
}