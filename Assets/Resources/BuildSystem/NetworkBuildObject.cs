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
    public bool CanBeBuildOnWall = false;
    public bool CanBeBuildOnSurface = true;
    // Start is called before the first frame update
    void Start()
    {
        _currentObject = Instantiate(PrefabToBuild);
        _currentObject.layer = 7;
        _currentObject.transform.position = transform.position;
        _currentObject.transform.parent = transform;
        _currentObject.transform.rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildObject(Vector3 position)
    {

    }
}
