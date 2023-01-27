using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    public delegate void OnGameEnd();
    public OnGameEnd onGameEnd;

    //Player Reference
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private DangerZone dangerZone;


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

        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();

            //If we still CANT find the player, we create one
            if (playerController == null)
            {
                //Create player here
            }
        }

        //If player movement aint initially assigned, we find it
        if (playerMovement == null) 
        {
            playerMovement = FindObjectOfType<PlayerMovement>();

            //If we still CANT find the player, we create one
            if (playerMovement == null) 
            {
                //Create player here
            }
        }

        //If dangerzone aint initially assigned, we find it
        if (playerMovement == null)
        {
            dangerZone = FindObjectOfType<DangerZone>();
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
        playerController.tempIsActive = true;
        gameState = GameState.InProcess;
    }

    void Update() 
    {
        if (gameState == GameState.InProcess)
        {
            if (PauseController.Instance.isPaused) { return; }

            //I believe .ToString() generates garbage, there might be a more beneficial way.
            gameTime += Time.deltaTime;
            SetTimeText(GetTimeString(gameTime));

            milesCounter +=  (playerMovement.currentSpeed / playerMovement.walkingSpeed) * Time.deltaTime;
            SetMilesText(Mathf.RoundToInt(milesCounter).ToString());
        }
    }

    public void EndGame() 
    {
        gameState = GameState.End;

        if (onGameEnd != null)
        {
            onGameEnd();
        }

        HUDManager.Instance.SetAfterScreenText(milesText.text, timeText.text);
        HUDManager.Instance.ToggleLostScreen();

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

    public PlayerController GetPlayerController() 
    {
        return playerController;
    }

    public void PlayAgain() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
