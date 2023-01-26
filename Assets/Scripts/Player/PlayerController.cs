using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [HideInInspector] public int batteries = 1;
    [HideInInspector] public int maxBatteries = 3;

    [SerializeField] private int tempDecrease = 1;
    [SerializeField] private int tempIncrease = 5;
    [HideInInspector] public int currentTemp = 50;
    [HideInInspector] public int fireTicks = 0;
    private float tempTimer = 1f;
    private Color defaultColor;

    private void Start() {
        maxBatteries = HUDManager.Instance.batteries.Length;
        defaultColor = GetComponent<SpriteRenderer>().color;
    }

    private void Update() {
        // Naturally, decrease the currentTemp by 1 every second. If the player is on fire, increase currentTemp by 1 every second.
        if (tempTimer > 0) {
            tempTimer -= Time.deltaTime;
        } else {
            if (fireTicks > 0) {
                if (fireTicks == 0) GetComponent<SpriteRenderer>().color = defaultColor;

                if (currentTemp >= 100) return;
                currentTemp += tempIncrease;
            } else {
                if (currentTemp <= 0) return;
                currentTemp -= tempDecrease;
            }
            HUDManager.Instance.UpdateTemperature(currentTemp);
            tempTimer = 1f;
        }
    }
}
