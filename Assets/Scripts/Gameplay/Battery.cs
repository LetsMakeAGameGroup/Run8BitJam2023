using UnityEngine;

public class Battery : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        // When the player collides with this Battery, check if the player has space to pick it up before doing so.
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (!playerController) return;

        if (playerController.batteries < playerController.maxBatteries) {
            playerController.batteries++;
            HUDManager.Instance.UpdateBatteries(playerController.batteries);
            Destroy(gameObject);
        } else {
            Destroy(this);
        }
    }
}
