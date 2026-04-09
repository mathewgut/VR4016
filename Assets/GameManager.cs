using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currCollected = 0;
    public int targetCollected = 5;

    public enum GameState
    {
        Won,
        Lost,
        Paused,
        Looking, // main state, means not all ducks found
        Collected // means all ducks found, exit door has appeared
    }

    public GameState gameState = GameState.Looking;

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
        if (gameState == GameState.Paused) Time.timeScale = 0;
        else Time.timeScale = 1f;

        if (gameState == GameState.Won) SceneManager.LoadScene(3);
        if (gameState == GameState.Lost) SceneManager.LoadScene(4);

        if (currCollected >= targetCollected && gameState != GameState.Collected) gameState = GameState.Collected;

        
    }


    public void IncrementCollected() {
        currCollected += 1;
    }

    public int GetCurrCollected() => currCollected;

    public int GetToCollect() => targetCollected;

    public GameState GetGameState() => gameState;

    public void SetGameState(GameState state) => gameState = state;
}
