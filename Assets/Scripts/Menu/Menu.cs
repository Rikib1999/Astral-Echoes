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
    public UnityEditor.SceneAsset play_scene;

    public override void OnNetworkSpawn()
    {
        NetworkManager.SceneManager.LoadScene(play_scene.name,LoadSceneMode.Single);
    }

    public void OnPlayButton ()
    {
        NetworkManager.Singleton.StartServer();
    }
    public void OnQuitButton ()
    {
        Application.Quit();
    }
    public void OnJoinButton ()
    {
        //Load multiplayer scene
        NetworkManager.GetComponent<UnityTransport>().SetConnectionData(tmp_ip_address.text,ushort.Parse(tmp_port.text));

        NetworkManager.Singleton.StartClient();
    }
}
