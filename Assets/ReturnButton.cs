using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class ReturnButton : NetworkBehaviour
{
    [SerializeField] public UnityEditor.SceneAsset return_scene;

    public void OnReturnButton()
    {
        if(!IsServer){return;}
        var status = NetworkManager.SceneManager.LoadScene(return_scene.name,LoadSceneMode.Single);
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to load return scene with a {nameof(SceneEventProgressStatus)}: {status}");
        }
    }
}
