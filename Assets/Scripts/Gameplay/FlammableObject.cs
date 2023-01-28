using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(Collider2D))]
public class FlammableObject : MonoBehaviour {
    private bool onFire = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (onFire) return;

        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (!playerController) return;

        if (playerController.IsOnFire()) {
            onFire = true;
            playerController.ReduceFire();
            GetComponent<ParticleSystem>().Play();
        }
    }
}
