using UnityEngine;

public class PlayerController : MonoBehaviour {
    [HideInInspector] public int batteries = 1;
    private int maxBatteries = 3;
    [HideInInspector] public int currentTemp = 50;

    private void Awake() {
        maxBatteries = HUDManager.Instance.batteries.Length;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // When colliding with a Battery, check if the player has space to pick it up before doing so.
        if (collision.CompareTag("Battery") && batteries < maxBatteries) {
            batteries++;
            HUDManager.Instance.UpdateBatteries(batteries);
            Destroy(collision.gameObject);
        }
    }
}
