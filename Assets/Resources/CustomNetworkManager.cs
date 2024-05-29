using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{

    public Dictionary<ulong, string> PlayerNames = new Dictionary<ulong, string>();
    private void Start()
    {
        
    }

    public void AddPlayerName(ulong clientId, string name)
    {
        PlayerNames.Add(clientId, name);
    }
    
    public void RemovePlayerName(ulong clientId)
    {
        PlayerNames.Remove(clientId);
    }

    
}
