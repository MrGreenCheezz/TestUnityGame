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
    // Start is called before the first frame update
    void Start()
    {
        if(Application.platform == RuntimePlatform.WindowsServer)
        {
            SceneManager.LoadScene("OnlineScene");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("127.0.0.1", 7777);
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
     
    public void JoinGame()
    {
        SceneManager.LoadScene("OnlineScene");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("127.0.0.1", 7777);
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
            playerObject.transform.position = new Vector3(10 * clientId, 0, 0);
        }
    }

    public void RequestName()
    {

    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} disconnected.");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
