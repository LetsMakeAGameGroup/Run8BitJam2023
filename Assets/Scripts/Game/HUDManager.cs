using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public static HUDManager Instance { get; private set; }

    [Header("Battery HUD Settings")]
    public Image[] batteries;
    public Image[] batteryLights;
    [SerializeField] private Sprite missingBattery;
    [SerializeField] private Sprite availableBattery;

    [Header("Temperature HUD Settings")]
    [SerializeField] private TextMeshProUGUI tempText;
    [SerializeField] private TextMeshProUGUI warningTimer;
    [SerializeField] private RectTransform tempGaugeTrans;
    [SerializeField] private GameObject heatWarning;
    [SerializeField] private GameObject coldWarning;

    public GameObject PauseMenu;
    public GameObject LostMenu;
    [SerializeField] private AudioClip buttonPressClip;

    //UI Stuff
    [SerializeField] TMP_Text loseTimeText;
    [SerializeField] TMP_Text loseMilesText;

    private void Awake() {
        if (Instance != null & Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    // Updates each battery in the HUD to represent how many batteries the player has.
    public void UpdateBatteries(int batteryCount) {
        for (int i = 0; i < batteries.Length; i++) {
            batteries[i].enabled = (i + 1 <= batteryCount ? true : false);
            batteryLights[i].sprite = (i + 1 <= batteryCount ? availableBattery : missingBattery);
        }
    }

    // Updates temperature text in the HUD, and position the gauge accordingly.
    public void UpdateTemperature(int temperature) {
        tempText.text = temperature.ToString() + "°";
        tempGaugeTrans.anchoredPosition = new Vector2(tempGaugeTrans.anchoredPosition.x, temperature * 6.8f + 200);
    }

    public void UpdateWarning(float time, bool isHot) {
        if (warningTimer.enabled == false) {
            warningTimer.enabled = true;
            if (isHot) {
                heatWarning.SetActive(true);
            } else {
                coldWarning.SetActive(true);
            }
        }
        warningTimer.text = Mathf.Floor(time+1).ToString();
    }

    public void DisableWarning() {
        warningTimer.enabled = false;
        heatWarning.SetActive(false);
        coldWarning.SetActive(false);
    }

    public void TogglePauseMenu() 
    {
        ButtonPressAudio();
        PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
    }

    public void ToggleLostScreen() 
    {
        LostMenu.SetActive(!LostMenu.activeInHierarchy);
    }

    public void SetAfterScreenText(string miles, string time) 
    {
        loseMilesText.text = miles;
        loseTimeText.text = time;
    }

    public void ButtonPressAudio() {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = buttonPressClip;
        audio.volume = (PlayerPrefs.HasKey("FXVolume") ? PlayerPrefs.GetFloat("FXVolume") / 100f : 0.5f);
        audio.Play();
    }
}
