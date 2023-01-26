using UnityEngine;

public class Fire : MonoBehaviour {
    [SerializeField] private int fireHP = 4;
    [SerializeField] private Color fireColor;

    private void OnTriggerEnter2D(Collider2D collision) {
        // When the player collides with this Fire, set the player on fire.
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (!playerController) return;

        collision.GetComponent<SpriteRenderer>().color = fireColor;
        playerController.fireTicks = fireHP;
        playerController.onFire = true;
        Destroy(gameObject);
    }
}
