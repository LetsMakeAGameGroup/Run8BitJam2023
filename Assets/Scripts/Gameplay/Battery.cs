using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(Collider2D))]
public class Battery : MonoBehaviour {
    [SerializeField] private GameObject model;
    private bool isPickedUp = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (isPickedUp) return;

        // When the player collides with this Battery, check if the player has space to pick it up before doing so.
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (!playerController) return;

        if (playerController.batteries < playerController.maxBatteries) {
            playerController.batteries++;
            HUDManager.Instance.UpdateBatteries(playerController.batteries);

            StartCoroutine(PickupBattery());
        }
    }

    IEnumerator PickupBattery() {
        isPickedUp = true;
        model.SetActive(false);

        ParticleSystem particles = GetComponent<ParticleSystem>();
        particles.Play();

        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = (PlayerPrefs.HasKey("FXVolume") ? PlayerPrefs.GetFloat("FXVolume") / 100f : 0.5f);
        audio.Play();

        while (particles.isPlaying || audio.isPlaying) {
            yield return null;
        }

        Destroy(gameObject);
    }
}
