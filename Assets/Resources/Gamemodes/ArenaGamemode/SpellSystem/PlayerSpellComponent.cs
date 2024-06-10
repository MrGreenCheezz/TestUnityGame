using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpellComponent : NetworkBehaviour
{
    public List<GameObject> SpellsList = new List<GameObject>();
    public List<BaseSpellScript> Spells = new List<BaseSpellScript>();
    public int CurrentSpellIndex = 0;
    public GameObject testSpawn;
    public GameObject PlayerSpellUIComponentPrefab;

    private GameObject PlayerSpellUIComponent;
    private SpellUI spellUiComponent;
    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner)
        {
            var tmpObj = Instantiate(PlayerSpellUIComponentPrefab);
            tmpObj.transform.parent = gameObject.transform;
            spellUiComponent = tmpObj.GetComponent<SpellUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (IsOwner)
            {
                AddSpellServerRpc(0);
            }
        }
    }

    public void ChangeSpellIndex(int index)
    {
        CurrentSpellIndex = index;
        spellUiComponent.SetSelectedSpell(index);
    }

    public void CastSpell()
    {
        Spells[CurrentSpellIndex].CastSpell();
    }

    public void AddSpell(GameObject spell)
    {
        if (IsOwner)
        {
           var indexTmp = SpellsList.IndexOf(spell);
            AddSpellServerRpc(indexTmp);
        }
       
    }
    [ServerRpc]
    public void AddSpellServerRpc(int spellListIndex)
    {
        var obj = SpellsList[spellListIndex];
        var tmp = Instantiate(obj, transform);
        tmp.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        tmp.transform.SetParent(transform);
        Spells.Add(tmp.GetComponent<BaseSpellScript>());
        AddSpellClientRpc(tmp.GetComponent<NetworkObject>());
    }
    [ClientRpc]
    public void AddSpellClientRpc(NetworkObjectReference obj)
    {
        obj.TryGet(out NetworkObject networkObject);
        Spells.Add(networkObject.gameObject.GetComponent<BaseSpellScript>());
        spellUiComponent.SetSpell(Spells.Count - 1, Spells[Spells.Count - 1].SpellImage);
    }
}
