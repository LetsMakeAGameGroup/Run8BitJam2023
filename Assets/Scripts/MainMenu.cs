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
    [SerializeField] private Slider fxSlider;
    [SerializeField] private AudioClip buttonPressClip;

    private void Awake() {
        if (PlayerPrefs.HasKey("MusicVolume")) {
            soundSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (PlayerPrefs.HasKey("FXVolume")) {
            fxSlider.value = PlayerPrefs.GetFloat("FXVolume");
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

    public void SetMusicVolume(float volume) {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        MusicManager.Instance.GetComponent<AudioSource>().volume = volume / 100f;
    }

    public void SetFXVolume(float volume) {
        PlayerPrefs.SetFloat("FXVolume", volume);
    }

    public void ButtonPressAudio() {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = buttonPressClip;
        audio.volume = (PlayerPrefs.HasKey("FXVolume") ? PlayerPrefs.GetFloat("FXVolume") / 100f : 0.5f);
        audio.Play();
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
