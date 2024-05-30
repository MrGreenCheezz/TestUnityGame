using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class PlayerNameSystemScript : NetworkBehaviour
{

    public NetworkVariable<FixedString128Bytes> MyPlayersName = new NetworkVariable<FixedString128Bytes>();
    public GameObject NameUIObject;
    public Camera PlayerCamera;
    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = Camera.current;
        if (IsServer)
        {
           // MyPlayersName.OnValueChanged += ChangePlayerNameClientRpc;
        }
        if (IsOwner)
        {
            SendNameServerRpc(Persistentdata.Instance.PlayerName);
            NameUIObject.SetActive(false);
            PlayerNameSystemScript[] Names = (PlayerNameSystemScript[])FindObjectsOfType(typeof(PlayerNameSystemScript));
            foreach (var name in Names)
            {
                if (name != this)
                {
                    name.PlayerCamera = GetComponentInChildren<Camera>();
                }
            }

        }
        else
        {

        }

        NameUIObject.GetComponent<TextMeshProUGUI>().text = MyPlayersName.Value.ToString();
    }
    private void OnEnable()
    {
        if (IsOwner)
        {
            PlayerNameSystemScript[] Names = (PlayerNameSystemScript[])FindObjectsOfType(typeof(PlayerNameSystemScript));
            foreach (var name in Names)
            {
                if (name != this)
                {
                    name.PlayerCamera = GetComponentInChildren<Camera>();
                }
            }
        }
    }
    public void ChangeCameraToFollow(Camera camera)
    {
        PlayerCamera = camera;
    }

    [ClientRpc]
    public void ChangePlayerNameClientRpc(FixedString128Bytes newName)
    {
        NameUIObject.GetComponent<TextMeshProUGUI>().text = newName.ToString();
    }

    [ServerRpc]
    public void SendNameServerRpc(string name)
    {
       MyPlayersName.Value = name;
       ChangePlayerNameClientRpc(name);
        var customManager = NetworkManager.Singleton as CustomNetworkManager;
        if(customManager.PlayerNames.ContainsKey(OwnerClientId))
        {
            customManager.PlayerNames[OwnerClientId] = name;
        }
        else
        {
            customManager.AddPlayerName(OwnerClientId, name);
        }
    }
    // Update is called once per frame


    public override void OnDestroy()
    {
        base.OnDestroy();
    }
    void LateUpdate()
    {
        if (PlayerCamera != null)
        {

                NameUIObject.transform.LookAt(NameUIObject.transform.position + PlayerCamera.transform.rotation * Vector3.forward,
                             PlayerCamera.transform.rotation * Vector3.up);
           
        }
    }

}
