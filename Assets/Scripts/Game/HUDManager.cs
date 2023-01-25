using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public static HUDManager Instance { get; private set; }

    [Header("Battery HUD Settings")]
    public Image[] batteries;
    [SerializeField] private Color missingBattery;
    [SerializeField] private Color availableBattery;

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
            batteries[i].color = (i + 1 <= batteryCount ? availableBattery : missingBattery);
        }
    }

    // Updates temperature text in the HUD, and position the gauge accordingly.
    public void UpdateTemperature(int temperature) {
        tempText.text = temperature.ToString() + "°";
        tempGaugeTrans.anchoredPosition = new Vector2(tempGaugeTrans.anchoredPosition.x, temperature * 3 - 5);
    }
}
