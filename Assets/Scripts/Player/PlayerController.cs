using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [HideInInspector] public int batteries = 1;
    private int maxBatteries = 3;

    [HideInInspector] public int currentTemp = 50;
    [HideInInspector] public int fireTicks = 0;
    private float tempTimer = 1f;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color fireColor;

    private void Start() {
        maxBatteries = HUDManager.Instance.batteries.Length;
    }

    private void Update() {
        // Naturally, decrease the currentTemp by 1 every second. If the player is on fire, increase currentTemp by 1 every second.
        if (tempTimer > 0) {
            tempTimer -= Time.deltaTime;
        } else {
            if (fireTicks > 0) {
                fireTicks--;
                if (fireTicks == 0) GetComponent<SpriteRenderer>().color = defaultColor;

                if (currentTemp >= 100) return;
                currentTemp++;
            } else {
                if (currentTemp <= 0) return;
                currentTemp--;
            }
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
            GetComponent<SpriteRenderer>().color = fireColor;
            fireTicks = 10;
            HUDManager.Instance.UpdateTemperature(currentTemp);
            Destroy(collision.gameObject);
        }
    }
}
