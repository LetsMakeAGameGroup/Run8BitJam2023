using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    [SerializeField] private float slowSpeedPerc;
    [SerializeField] private float slowTime;

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (!playerController) return;

        playerController.SetOffFire();

        collision.GetComponent<PlayerMovement>().ReduceCurrentSpeedByTime(slowSpeedPerc, slowTime);
        Destroy(gameObject);
    }
}
