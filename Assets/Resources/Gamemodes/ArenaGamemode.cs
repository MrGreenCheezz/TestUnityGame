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

    public ArenaGamemode GetSingleton()
    {
        return instance;
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
