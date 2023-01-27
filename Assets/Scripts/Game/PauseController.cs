using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance { get; private set; }

    public bool isPaused;
    public GameObject PauseMenuUI;

    void Awake()
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

    public void TogglePause() 
    {
        isPaused = !isPaused;

        HUDManager.Instance.TogglePauseMenu();
    }

    public void Continue() 
    {
        TogglePause();
    }

    public void BackToMenu() 
    {
        SceneManager.LoadScene(0);
    }
}
