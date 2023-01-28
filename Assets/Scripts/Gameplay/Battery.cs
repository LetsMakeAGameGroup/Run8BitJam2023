using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Battery : MonoBehaviour {
    [SerializeField] private GameObject pickupParticles;

    private void OnTriggerEnter2D(Collider2D collision) {
        // When the player collides with this Battery, check if the player has space to pick it up before doing so.
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (!playerController) return;

        if (playerController.batteries < playerController.maxBatteries) {
            playerController.batteries++;
            HUDManager.Instance.UpdateBatteries(playerController.batteries);
            GameObject particlesObject = Instantiate(pickupParticles, transform.position, Quaternion.identity);
            ParticleSystem particles = particlesObject.GetComponent<ParticleSystem>();
            particles.Play();
            Destroy(particlesObject, particles.main.duration);
            Destroy(gameObject);
        }
    }
}
