using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkBaseSetupScript : MonoBehaviour
{

    public List<GameObject> AvailableGamemodes = new List<GameObject>();
    public string OfflineScene;
    public string OnlineScene;
    [SerializeField]
    private GameObject spawnPos;
    // Start is called before the first frame update
    void Start()
    {
        if(Application.platform == RuntimePlatform.WindowsServer)
        {
            SceneManager.sceneLoaded += SceneChanged;
            SceneManager.LoadScene("OnlineScene");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("26.166.14.33", 7777);
            NetworkManager.Singleton.StartServer();
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;

            foreach (var gamemode in AvailableGamemodes)
            {
                var tmp = Instantiate(gamemode);
                tmp.GetComponent<NetworkObject>().Spawn();
            }
           
        }
       

    }

    private void SceneChanged(Scene scene, LoadSceneMode sceneMode)
    {
        if(scene.name == OnlineScene)
        {
            spawnPos = GameObject.FindWithTag("Respawn");
        }
    }

 
     
    public void JoinGame()
    {
        SceneManager.LoadScene("OnlineScene");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("26.166.14.33", 7777);
        NetworkManager.Singleton.StartClient();
    }

    private void OnDisable()
    {
        if (Application.platform == RuntimePlatform.WindowsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log($"Client {clientId} connected.");
            var playerObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);
            playerObject.GetComponent<FirstPersonControllerNetworked>().MovePlayerToPositionClientRpc(spawnPos.transform.position);
        }
    }

    public void RequestName()
    {

    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} disconnected.");
        (NetworkManager.Singleton as CustomNetworkManager).RemovePlayerName(clientId);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
