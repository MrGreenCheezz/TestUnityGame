using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public enum ArenaState
{
    WaitingForPlayers,
    Preparing,
    InProgress,
    Finished
}
public class ArenaGamemode : NetworkBehaviour, IGameMode<ArenaGamemode>
{
    public static ArenaGamemode instance;
    
    public NetworkVariable<ArenaState> CurrentState = new NetworkVariable<ArenaState>(ArenaState.WaitingForPlayers);

    public Dictionary<ulong, int> PlayerScores = new Dictionary<ulong, int>();
    public Dictionary<ulong, GameObject> PlayersOldGameobjects = new Dictionary<ulong, GameObject>();
    public Dictionary<ulong, GameObject> PlayersCurrentGameobjects = new Dictionary<ulong, GameObject>();

    public int ScoreToWin = 5;

    public GameObject PlayerPrefab;

    public ArenaGamemode GetSingleton()
    {
        return instance;
    }

    public void PlayerJoined(ulong clientId)
    {
        PlayerScores.Add(clientId, 0);
        MigratePlayer(clientId, true);
    }

    public void PlayerLeft(ulong clientId)
    {
        PlayerScores.Remove(clientId);
        MigratePlayer(clientId, false);
    }

    public void MigratePlayer(ulong clientId, bool isPlayerJoiningGamemode)
    {
        if (isPlayerJoiningGamemode)
        {
            var tmp = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId).gameObject;
            tmp.SetActive(false);
            DeactivatePlayerObjectClientRpc(tmp.GetComponent<NetworkObject>());         
            PlayersOldGameobjects.Add(clientId, tmp);
            var player = Instantiate(PlayerPrefab);    
            player.transform.position = GameObject.Find("ArenaSpawnPoint").transform.position;
            player.AddComponent<HpComponent>();
            player.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            tmp.GetComponent<NetworkObject>().TrySetParent(player);
            PlayersCurrentGameobjects.Add(clientId, player);
            
        }
        else
        {
            var tmp = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId).gameObject;
            tmp.GetComponent<NetworkObject>().TrySetParent(PlayersOldGameobjects[clientId]);
            
            //PlayersOldGameobjects[clientId].SetActive(true);
            ActivatePlayerObjectClientRpc(PlayersOldGameobjects[clientId].GetComponent<NetworkObject>());
            Destroy(PlayersCurrentGameobjects[clientId]);
            PlayersCurrentGameobjects.Remove(clientId);
            PlayersOldGameobjects.Remove(clientId);

        }
    }

    [ClientRpc]
    public void DeactivatePlayerObjectClientRpc(NetworkObjectReference obj)
    {
        if(obj.TryGet(out NetworkObject networkObject))
        {
            networkObject.gameObject.SetActive(false);
        }
    }
    [ClientRpc]
    public void ActivatePlayerObjectClientRpc(NetworkObjectReference obj)
    {
        if (obj.TryGet(out NetworkObject networkObject))
        {
            networkObject.gameObject.SetActive(true);
           
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void OnEnable()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
