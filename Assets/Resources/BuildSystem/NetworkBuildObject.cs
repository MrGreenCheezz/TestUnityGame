using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum BuildState
{
    Building,
    None
}

public class NetworkBuildObject : NetworkBehaviour
{
    public GameObject PrefabToBuild;
    private GameObject _currentObject;
    public GameObject buildGhostObject;

    // Start is called before the first frame update
    void Start()
    {
        _currentObject = Instantiate(PrefabToBuild);
        _currentObject.transform.position = transform.position;
        _currentObject.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildObject(Vector3 position)
    {

    }
}
