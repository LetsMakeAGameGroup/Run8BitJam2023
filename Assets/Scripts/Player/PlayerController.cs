using UnityEngine;

public class PlayerController : MonoBehaviour {
    [HideInInspector] public int batteries = 1;
    private int maxBatteries = 3;

    [HideInInspector] public int currentTemp = 50;
    [SerializeField] private int fireIncrease = 15;

    private float tempTimer = 1f;

    private void Start() {
        maxBatteries = HUDManager.Instance.batteries.Length;
    }

    private void Update() {
        if (currentTemp <= 0) return;

        // Decrease the currentTemp by 1 every second.
        if (tempTimer > 0) {
            tempTimer -= Time.deltaTime;
        } else {
            currentTemp--;
            HUDManager.Instance.UpdateTemperature(currentTemp);
            tempTimer = 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // When colliding with a Battery, check if the player has space to pick it up before doing so.
        if (collision.CompareTag("Battery") && batteries < maxBatteries) {
            batteries++;
            HUDManager.Instance.UpdateBatteries(batteries);
            Destroy(collision.gameObject);
        }

        // When colliding with Fire, increase the player's temperature by fireIncrease.
        if (collision.CompareTag("Fire")) {
            if (currentTemp + fireIncrease > 100) currentTemp = 100;
            else currentTemp += fireIncrease;
            HUDManager.Instance.UpdateTemperature(currentTemp);
            Destroy(collision.gameObject);
        }
    }
}
