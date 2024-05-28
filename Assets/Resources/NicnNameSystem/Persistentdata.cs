using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistentdata : MonoBehaviour
{
    public static Persistentdata Instance;

    public string PlayerName;
    private void OnEnable()
    {
        //Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerName(string name)
    {
        PlayerName = name;
    }
}
