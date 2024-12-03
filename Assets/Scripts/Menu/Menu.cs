using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : NetworkBehaviour
{
    [SerializeField] public TMP_InputField tmp_ip_address;
    [SerializeField] public TMP_InputField tmp_port;

    public override void OnNetworkSpawn()
    {
        if(!IsServer){return;}
        var status = NetworkManager.SceneManager.LoadScene("SpaceMap", LoadSceneMode.Single);
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