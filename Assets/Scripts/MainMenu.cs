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
        if (PlayerPrefs.HasKey("SoundVolume")) {
            soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
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

    public void SetSoundVolume(float volume) {
        PlayerPrefs.SetFloat("SoundVolume", volume);
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
}
