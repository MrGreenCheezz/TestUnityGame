using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class BaseSpellScript : NetworkBehaviour
{
    public bool isReloading = false;
    private float reloadTime = 0.0f;
    public float TimeToReload = 1.0f;
    public Sprite SpellImage;
    public int Damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   public virtual void Update()
    {
        if(IsOwner || IsServer)
        {
            if (isReloading)
            {
                reloadTime += Time.deltaTime;
                if (reloadTime >= TimeToReload)
                {
                    isReloading = false;
                    reloadTime = 0.0f;
                }
            }
        }    
    }

    public virtual void CastSpell()
    {
        if (!isReloading)
        {
            StartReloading();
        }
    }

    public void StartReloading()
    {
        isReloading = true;
    }
}
