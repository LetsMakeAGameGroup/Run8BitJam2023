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
    [SerializeField] private RectTransform tempGaugeTrans;

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
}
