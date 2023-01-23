using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState 
{
    Starting,
    InProcess,
    End
}

// SUMMARY:
//  GameMode will be incharge of handeling the rules of the game. When it starts, the process and end.
//

public class GameMode : MonoBehaviour
{
    public static GameMode Instance { get; private set; }
    public GameState gameState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void StartGame() { }

    void InProcessUpdate() { }

    void EndGame() { }

}
