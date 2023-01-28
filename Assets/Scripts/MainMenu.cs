using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;

    [SerializeField] private Slider soundSlider;

    private void Awake() {
        if (PlayerPrefs.HasKey("SoundVolume")) {
            soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        }
    }

    public void StartGame() 
    {
        SceneManager.LoadScene(1);
    }

    public void OpenOptionsMenu() 
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void SetSoundVolume(float volume) {
        PlayerPrefs.SetFloat("SoundVolume", volume);
        MusicManager.Instance.GetComponent<AudioSource>().volume = volume / 100f;
    }
}
