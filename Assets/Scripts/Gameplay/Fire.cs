using UnityEngine;

public class Fire : MonoBehaviour {
    [SerializeField] private int fireHP = 4;

    private void OnTriggerEnter2D(Collider2D collision) {
        // When the player collides with this Fire, set the player on fire.
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (!playerController) return;

        playerController.SetOnFire(fireHP);

        PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
        playerMovement.slowCount = 0;
        playerMovement.SetSlowedSpeed();
        Destroy(gameObject);
    }
}
