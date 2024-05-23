using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayersNetworkBuildComponent : NetworkBehaviour
{
    public NetworkBuildObject[] BuildObjects;
    public int CurrentBuildObjectIndex;
    private RaycastHit _hit;
    private FirstPersonControllerNetworked _controller;
    private bool _isBuilding = false;
    private GameObject _buildingObjectGhost;
    public Material okMaterial;
    public Material notOkMaterial;
    private BuildGhostChecker _currentGhost;
    // Start is called before the first frame update
    void Start()
    {
       _controller = GetComponent<FirstPersonControllerNetworked>();
    }

    // Update is called once per frame
    void Update()
    {
        
                  
        if (Input.GetKeyDown(KeyCode.B))
        {
           SwitchBuildingMode();
        }
        if (_isBuilding)
        {
            Vector3 direction = _controller.playerCamera.transform.forward * 200f;
            if (Physics.Raycast(_controller.playerCamera.transform.position, direction, out _hit, 500, ((1 << 6) | (1 << 7))))
            {
                _buildingObjectGhost.transform.position = _hit.point;
                _currentGhost.normal = _hit.normal;
                //Debug.DrawLine(transform.position, transform.position + direction * 200f, Color.red, 5f);
            }
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (_buildingObjectGhost.GetComponent<BuildGhostChecker>().canBuild)
                {
                    BuildObject(_hit.point);
                }
            }
        }
    }
    public void SwitchBuildingMode()
    {
        _isBuilding = !_isBuilding;
        if (_isBuilding)
        {
            if(BuildObjects[CurrentBuildObjectIndex].buildGhostObject == null)
            {
                _buildingObjectGhost = Instantiate(BuildObjects[CurrentBuildObjectIndex].PrefabToBuild);
                _buildingObjectGhost.AddComponent<Rigidbody>().isKinematic = true;
                var tmp = _buildingObjectGhost.AddComponent<BuildGhostChecker>();
                tmp.okMaterial = okMaterial;
                tmp.notOkMaterial = notOkMaterial;
                tmp.ParentNetBuildObject = BuildObjects[CurrentBuildObjectIndex];
                _currentGhost = tmp;
            }
            else
            {
                _buildingObjectGhost = Instantiate(BuildObjects[CurrentBuildObjectIndex].buildGhostObject);
                _currentGhost = _buildingObjectGhost.GetComponent<BuildGhostChecker>();
            }
            
        }
        else
        {
            Destroy(_buildingObjectGhost);
        }
    }
    public void ChangeSelectedObject(int index)
    {
        CurrentBuildObjectIndex = index;
    }

    public void BuildObject(Vector3 position)
    {
        SwitchBuildingMode();
        if (CurrentBuildObjectIndex < BuildObjects.Length)
        {
            BuildObjectRpc(position);
        }
    }
    [Rpc(SendTo.Server)]
    public void BuildObjectRpc(Vector3 position)
    {
        var tmpObject = Instantiate(BuildObjects[CurrentBuildObjectIndex], position, Quaternion.identity);
        tmpObject.GetComponent<NetworkObject>().Spawn();
    }
}
