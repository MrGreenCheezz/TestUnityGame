using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildGhostChecker : MonoBehaviour
{
    public MeshRenderer[] renders;
    public List<Material> materials = new List<Material>();
    public Material okMaterial;
    public Material notOkMaterial;
    public bool canBuild = true;
    private void Start()
    {
      renders =  gameObject.GetComponents<MeshRenderer>();
       foreach(var render in renders)
        {
           render.material = okMaterial;
       }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        print("Collision");
        foreach (var render in renders)
        {
            render.material = notOkMaterial;
        }
        canBuild = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        print("Exit");
        foreach (var render in renders)
        {
            render.material = okMaterial;
        }
        canBuild = true;
    }
}
