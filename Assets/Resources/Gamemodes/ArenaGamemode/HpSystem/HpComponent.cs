using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HpComponent : NetworkBehaviour
{
    //Просто перенести на персонажа сам код компонента и может быть получиться а пока просто не получается создать объект с Интерфейсом здоровья
    public GameObject CanvasToAdd;
    public GameObject HpUiPrefab;
    [SerializeField]
    private Canvas localCanvas;
    [SerializeField]
    private GameObject localHpUi;
    [SerializeField]
    private Image fillerImage;

    //private Vector3 HpBarCoords = new Vector3(225, 50, 0);

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

            if (IsOwner)
            {
                localCanvas = Instantiate(CanvasToAdd, gameObject.transform).GetComponent<Canvas>();
                print(localCanvas);
                localHpUi = Instantiate(HpUiPrefab, localCanvas.transform);
                fillerImage = localHpUi.transform.Find("Filler").GetComponent<Image>();
                
                MaxHP.OnValueChanged += OwnerMaxHpChanged;
                CurrentHP.OnValueChanged += OwnerCurrentHpChanged;
            }
            else
            {

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
