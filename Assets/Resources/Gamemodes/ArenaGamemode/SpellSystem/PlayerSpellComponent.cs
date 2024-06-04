using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpellComponent : NetworkBehaviour
{
    public List<BaseSpellScript> Spells = new List<BaseSpellScript>();
    public int CurrentSpellIndex = 0;
    public GameObject testSpawn;
    // Start is called before the first frame update
    void Start()
    {
        AddSpell(testSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSpellIndex(int index)
    {
        CurrentSpellIndex = index;
    }

    public void CastSpell()
    {
        Spells[CurrentSpellIndex].CastSpell();
    }

    public void AddSpell(GameObject spell)
    {
        if (IsServer)
        {
            AddSpellServer(spell);
        }
    }
    public void AddSpellServer(GameObject obj)
    {
        var tmp = Instantiate(obj, transform);
        tmp.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        tmp.transform.SetParent(transform);
        AddSpellClientRpc(tmp.GetComponent<NetworkObject>());
    }
    [ClientRpc]
    public void AddSpellClientRpc(NetworkObjectReference obj)
    {
        obj.TryGet(out NetworkObject networkObject);
        Spells.Add(networkObject.gameObject.GetComponent<BaseSpellScript>());
    }
}
