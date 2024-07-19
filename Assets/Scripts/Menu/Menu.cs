using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class Menu : NetworkBehaviour
{
    [SerializeField] public TMP_InputField tmp_ip_address;
    [SerializeField] public TMP_InputField tmp_port;
    [SerializeField] public UnityEditor.SceneAsset play_scene;

    public override void OnNetworkSpawn()
    {
        if(!IsServer){return;}
        var status = NetworkManager.SceneManager.LoadScene(play_scene.name,LoadSceneMode.Single);
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to load play scene with a {nameof(SceneEventProgressStatus)}: {status}");
        }
    }

    public void OnPlayButton()
    {
        NetworkManager.Singleton.StartHost();
    }
    public void OnQuitButton()
    {
        Application.Quit();
    }
    public void OnJoinButton()
    {
        //Load multiplayer scene
        NetworkManager.GetComponent<UnityTransport>().SetConnectionData(tmp_ip_address.text,ushort.Parse(tmp_port.text));

        NetworkManager.Singleton.StartClient();
    }
}
