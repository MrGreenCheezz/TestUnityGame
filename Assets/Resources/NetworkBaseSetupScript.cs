using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEditor;
using UnityEngine;

public class NetworkBaseSetupScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(Application.platform == RuntimePlatform.WindowsServer)
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("192.168.0.133", 7777);
            NetworkManager.Singleton.StartServer();

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
           
        }
        else
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("192.168.0.133", 7777);
            NetworkManager.Singleton.StartClient();
        }
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
        Debug.Log($"Client {clientId} connected.");
        var playerObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);
        playerObject.transform.position = new Vector3(1 * clientId, 0, 0);
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} disconnected.");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("127.0.0.1", 7777);
            NetworkManager.Singleton.StartHost();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("127.0.0.1", 7777);
            NetworkManager.Singleton.StartClient();
        }
    }
}
