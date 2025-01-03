using Assets.Scripts;
using Assets.Scripts.SpaceSystem;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnButton : NetworkBehaviour
{
    public void OnReturnButton()
    {
        if (!IsServer) return;

        SystemMapManager.Instance.CentralObject = new NetworkVariable<SpaceObjectDataBag> { Value = new SpaceObjectDataBag() { Type = 0 } };

        var status = NetworkManager.SceneManager.LoadScene("SpaceMap", LoadSceneMode.Single);

        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to load return scene with a {nameof(SceneEventProgressStatus)}: {status}");
        }
    }
}