using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currCollected = 0;
    public int targetCollected = 5;

    public static GameManager Instance { get; private set; }

   
    void Awake()
    {
      
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void IncrementCollected() {
        currCollected += 1;
    }

    public int GetCurrCollected() => currCollected;

    public int GetToCollect() => targetCollected;
}
