using UnityEngine;

public class Door : MonoBehaviour {
    [SerializeField] private int health = 10;
    private bool isTriggered = false;

    private void Update() {
        if (isTriggered && Input.GetButtonDown("Submit")) health--;

        if (health <= 0) DestroyDoor();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (!playerController) return;

        if (playerController.IsOnFire()) {
            DestroyDoor();
        } else {
            isTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (!playerController) return;

        isTriggered = false;
    }

    private void DestroyDoor() {
        Destroy(gameObject);
    }
}
