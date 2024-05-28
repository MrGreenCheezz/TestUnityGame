using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    private void Start()
    {
        
    }

    public void StartClientConnection()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("127.0.0.1", 7777);
        UnityEngine.SceneManagement.SceneManager.LoadScene("OnlineScene");
        StartClient();
    }
}
