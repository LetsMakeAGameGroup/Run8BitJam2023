using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Obstacle : MonoBehaviour {
    [SerializeField] private float slowSpeedPerc;
    [SerializeField] private float slowTime;

    [SerializeField] private GameObject model;

    private bool isPickedUp = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (isPickedUp) return;

        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (!playerController) return;

        playerController.SetOffFire();

        collision.GetComponent<PlayerMovement>().ReduceCurrentSpeedByTime(slowSpeedPerc, slowTime);
        StartCoroutine(CollideObstacle());
    }

    IEnumerator CollideObstacle() {
        isPickedUp = true;
        model.SetActive(false);

        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = (PlayerPrefs.HasKey("FXVolume") ? PlayerPrefs.GetFloat("FXVolume") / 100f : 0.5f);
        audio.Play();

        while (audio.isPlaying) {
            yield return null;
        }

        Destroy(gameObject);
    }
}
