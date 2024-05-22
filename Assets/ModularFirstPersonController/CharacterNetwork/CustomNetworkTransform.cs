using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class CustomNetworkTransform : NetworkTransform
{
    public bool IsServerAuthoritiveSync = false;


    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
