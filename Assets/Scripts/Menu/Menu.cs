using Assets.Scripts;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : NetworkBehaviour
{
    [SerializeField] public TMP_InputField tmp_ip_address;
    [SerializeField] public TMP_InputField tmp_port;
    [SerializeField] public TMP_InputField tmp_port_host;
    public TMP_Text highscores;

    private void Start()
    {
        if (PlanetMapManager.Instance != null) PlanetMapManager.Instance.DestroyInstance();
        if (SystemMapManager.Instance != null) SystemMapManager.Instance.DestroyInstance();
        NetworkManager.Singleton.OnClientStopped += OnClientStopped;
    }

    public override void OnNetworkSpawn()
    {
        if(!IsServer){return;}

        SeedManager.Instance.StringToSeedSave();

        var status = NetworkManager.SceneManager.LoadScene("SpaceMap", LoadSceneMode.Single);
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to load play scene with a {nameof(SceneEventProgressStatus)}: {status}");
        }
    }

    public void OnPlayButton()
    {
        ushort port_host = 7777;
        if(tmp_port_host.text.Length > 0){
            port_host = ushort.Parse(tmp_port_host.text);
        }
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            "127.0.0.1",  //IP
            port_host,
            "0.0.0.0" //Listen on all addresses
        );

        NetworkManager.Singleton.StartHost();
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnResetButton()
    {
        int hs_dist = PlayerPrefs.GetInt("hs_dist", 0);
        int hs_enem = PlayerPrefs.GetInt("hs_enem", 0);
        int hs_res = PlayerPrefs.GetInt("hs_res", 0);

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("hs_dist", hs_dist);
        PlayerPrefs.SetInt("hs_enem", hs_enem);
        PlayerPrefs.SetInt("hs_res", hs_res);
    }

    public void OnResetHighscoreButton()
    {
        int cs_dist = PlayerPrefs.GetInt("cs_dist", 0);
        int cs_enem = PlayerPrefs.GetInt("cs_enem", 0);
        int cs_res = PlayerPrefs.GetInt("cs_res", 0);

        PlayerPrefs.SetInt("hs_dist", cs_dist);
        PlayerPrefs.SetInt("hs_enem", cs_enem);
        PlayerPrefs.SetInt("hs_res", cs_res);

        highscores.text = "HIGHSCORE:\n\nMax distance traveled:\n" + cs_dist + "\n\nEnemies killed:\n" + cs_enem + "\n\nResources gathered:\n" + cs_res;
    }

    public void OnJoinButton()
    {
        //Load multiplayer scene
        NetworkManager.GetComponent<UnityTransport>().SetConnectionData(tmp_ip_address.text,ushort.Parse(tmp_port.text));

        NetworkManager.Singleton.StartClient();

    }
    private void OnClientStopped(bool host)
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}