using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildGhostChecker : MonoBehaviour
{
    public NetworkBuildObject ParentNetBuildObject;
    public MeshRenderer[] renders;
    public List<Material> materials = new List<Material>();
    public Material okMaterial;
    public Material notOkMaterial;
    public bool canBuild = true;
    public Vector3 normal;
    private int collisionCount = 0;
    private bool isColliding;

    private void Start()
    {
      renders =  gameObject.GetComponents<MeshRenderer>();
       foreach(var render in renders)
        {
           render.material = okMaterial;
       }
        var collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    private void Update()
    {
        var tmpAngle = CheckPlaceAngle(normal);
        if(canBuild != tmpAngle)
        {
            canBuild = tmpAngle;
            ChangeMatAccordingBool(canBuild);
        }
    }

    public bool CheckPlaceAngle(Vector3 normal)
    {
  
        float angle = Vector3.Angle(normal, transform.up);

        if (isColliding)
        {
            return false;
        }
        if (angle < 35 && ParentNetBuildObject.CanBeBuildOnSurface)
        {
            return true;
        }
        if (angle > 70 && angle < 120 && ParentNetBuildObject.CanBeBuildOnWall)
        {
            return true;
        }
        return false;
    }

    private void ChangeMatAccordingBool(bool value)
    {
        foreach (var render in renders)
        {
            if (value)
            {
                render.material = okMaterial;
            }
            else
            {
                render.material = notOkMaterial;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionCount++;
        UpdateCollisionsState();
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionCount--;
        UpdateCollisionsState();
    }

    private void OnTriggerEnter(Collider other)
    {
        collisionCount++;
        UpdateCollisionsState();
    }

    private void OnTriggerExit(Collider other)
    {
        collisionCount--;
        UpdateCollisionsState();
    }

    private void UpdateCollisionsState()
    {
        isColliding = collisionCount > 0;
    }
}
