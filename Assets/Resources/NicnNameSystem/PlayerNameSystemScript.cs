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
    // Start is called before the first frame update
    void Start()
    {
        if (IsClient)
        {
            MyPlayersName.OnValueChanged += NameChanged;
        }
        if (IsOwner)
        {
            SendNameServerRpc(Persistentdata.Instance.PlayerName);
        }
    }

    public void NameChanged(FixedString128Bytes prev, FixedString128Bytes current)
    {
        NameUIObject.GetComponent<TextMeshPro>().text = current.ToString();
    }

    [ServerRpc]
    public void SendNameServerRpc(string name)
    {
       MyPlayersName.Value = name;
    }
    // Update is called once per frame
  

}
