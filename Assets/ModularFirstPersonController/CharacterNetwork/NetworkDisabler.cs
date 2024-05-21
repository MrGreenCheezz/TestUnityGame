using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkDisabler : MonoBehaviour
{
    NetworkObject networkObject;
    public Behaviour[] ComponentsToDisable;
    public MonoBehaviour[] MonoBehavioursToDisable;
    // Start is called before the first frame update
    void Start()
    {
        networkObject = GetComponent<NetworkObject>();
        if (!networkObject.IsOwner)
        {
            {

            }
            foreach (var comp in ComponentsToDisable)
            {
                comp.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
