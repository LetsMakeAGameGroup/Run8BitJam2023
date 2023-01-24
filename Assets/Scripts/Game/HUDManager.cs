using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public static HUDManager Instance { get; private set; }

    [Header("Battery HUD Settings")]
    public Image[] batteries;
    public Color missingBattery;
    public Color availableBattery;

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
}
