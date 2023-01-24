using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameState 
{
    Initialize,
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

    [SerializeField] float startGameTimer = 3;
    float gameTime;
    float milesCounter;
    string gameEndTime;

    //UI Stuff
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text milesText;

    //Delegates
    public delegate void OnGameStart();
    public OnGameStart onGameStart;

    //Player Reference
    [SerializeField] private PlayerMovement playerMovement;

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

        //If player controller aint initially assigned, we find it
        if (playerMovement == null) 
        {
            playerMovement = FindObjectOfType<PlayerMovement>();

            //If we still CANT find the player, we create one
            if (playerMovement == null) 
            {
                //Create player here
            }
        }
    }

    private void Start()
    {
        StartCoroutine(StartGameCountdown(startGameTimer));
    }

    IEnumerator StartGameCountdown(float Seconds) 
    {
        yield return new WaitForSeconds(Seconds);

        StartGame();

        yield return null;
    }

    void StartGame() 
    {
        Debug.Log("START!");

        if (onGameStart != null) 
        {
            onGameStart();
        }

        playerMovement.canMove = true;
        gameState = GameState.InProcess;
    }

    void Update() 
    {
        if (gameState == GameState.InProcess)
        {
            //I believe .ToString() generates garbage, there might be a more beneficial way.
            gameTime += Time.deltaTime;
            SetTimeText(GetTimeString(gameTime));

            milesCounter += Time.deltaTime;
            SetMilesText(Mathf.RoundToInt(milesCounter).ToString());
        }
    }

    public void EndGame() 
    {
        gameState = GameState.End;

        //Bring up Losing Screen
    }

    string GetTimeString(float time) 
    {
        time = Mathf.Max(0, time - Time.deltaTime);
        var timeSpan = System.TimeSpan.FromSeconds(time);
        return timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00") + "." + timeSpan.Milliseconds / 100;
    }

    public void SetTimeText(string newText)
    {
        if (timeText == null)
        {
            return;
        }

        timeText.text = newText;
    }

    public void SetMilesText(string newText)
    {
        if (milesText == null) 
        {
            return;
        }

        milesText.text = newText;
    }

}
