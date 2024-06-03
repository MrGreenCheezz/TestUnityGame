using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HpComponent : NetworkBehaviour
{
    //Нельзя просто так добавлять компонент, нужно сделать отдельный префаб , который будет содержать этот компонент
    public GameObject CanvasToAdd;
    public GameObject HpUiPrefab;
    private Canvas localCanvas;
    private GameObject localHpUi;
    private Image fillerImage;

    private Vector3 HpBarCoords = new Vector3(225, 50, 0);

    public NetworkVariable<int> MaxHP = new NetworkVariable<int>();
    public NetworkVariable<int> CurrentHP = new NetworkVariable<int>();
    // Start is called before the first frame update
    void Start()
    {
       
        if (IsServer)
        {
            MaxHP.OnValueChanged += ServerMaxHpChanged;
            CurrentHP.OnValueChanged += ServerCurrentHpChanged;
        }
        else
        {
            if (IsOwner)
            {
                localCanvas = gameObject.GetComponentInChildren<Canvas>();
                if (localCanvas == null || localCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
                {
                    localCanvas = Instantiate(CanvasToAdd, gameObject.transform).GetComponent<Canvas>();                  
                }
                localHpUi = Instantiate(HpUiPrefab, localCanvas.transform);
                fillerImage = transform.Find("Filler").GetComponent<Image>();
                MaxHP.OnValueChanged += OwnerMaxHpChanged;
                CurrentHP.OnValueChanged += OwnerCurrentHpChanged;
            }
            else
            {

            }
        }
    }
    [ServerRpc]
    public void SetMaxHpServerRpc(int amount)
    {
        MaxHP.Value = amount;
    }
    [ServerRpc]
    public void SetCurrentHpServerRpc(int amount)
    {
        CurrentHP.Value = amount;
    }
    [ServerRpc]
    public void TakeDamageServerRpc(int amount)
    {
        CurrentHP.Value -= amount;
    }
    [ServerRpc]
    public void HealServerRpc(int amount)
    {
        CurrentHP.Value += amount;
    }
    private void OwnerCurrentHpChanged(int previousValue, int newValue)
    {
        fillerImage.fillAmount = CurrentHP.Value / MaxHP.Value;
    }

    private void OwnerMaxHpChanged(int previousValue, int newValue)
    {
        
    }

    private void ServerCurrentHpChanged(int previousValue, int newValue)
    {
        
    }

    private void ServerMaxHpChanged(int previousValue, int newValue)
    {
       
    }

    public void SetMaxHp(int amount)
    {

    }

    public void SetCurrentHP(int amount)
    {

    }

    public void TakeDamage(int amount)
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnDestroy()
    {
        if (IsServer)
        {
            MaxHP.OnValueChanged -= ServerMaxHpChanged;
            CurrentHP.OnValueChanged -= ServerCurrentHpChanged;
        }
        else
        {
            if (IsOwner)
            {
                MaxHP.OnValueChanged -= OwnerMaxHpChanged;
                CurrentHP.OnValueChanged -= OwnerCurrentHpChanged;
            }
        }
        base.OnDestroy();
    }
}
