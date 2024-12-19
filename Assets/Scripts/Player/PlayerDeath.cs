using Assets.Scripts.SpaceSystem;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Player
{
    public class PlayerDeath : NetworkBehaviour
    {
        public void Die()
        {
            int hs_dist = PlayerPrefs.GetInt("hs_dist", 0);
            int hs_enem = PlayerPrefs.GetInt("hs_enem", 0);
            int hs_res = PlayerPrefs.GetInt("hs_res", 0);

            PlayerPrefs.DeleteAll();

            PlayerPrefs.SetInt("hs_dist", hs_dist);
            PlayerPrefs.SetInt("hs_enem", hs_enem);
            PlayerPrefs.SetInt("hs_res", hs_res);

            SystemMapManager.Instance.CentralObject = new NetworkVariable<SpaceObjectDataBag> { Value = new SpaceObjectDataBag() { Type = 0 } };
            SystemMapManager.Instance.Death = true;
            var status = NetworkManager.SceneManager.LoadScene("SpaceMap", LoadSceneMode.Single);
        }
    }
}