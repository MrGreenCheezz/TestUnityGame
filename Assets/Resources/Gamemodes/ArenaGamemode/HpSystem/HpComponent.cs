using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HpComponent : NetworkBehaviour
{
    public GameObject CanvasToAdd;
    public GameObject HpUiPrefab;
    private Canvas localCanvas;
    private GameObject localHpUi;
    // Start is called before the first frame update
    void Start()
    {
        localCanvas = gameObject.GetComponentInChildren<Canvas>();
        if(localCanvas == null || localCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            localCanvas = Instantiate(CanvasToAdd, gameObject.transform).GetComponent<Canvas>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
