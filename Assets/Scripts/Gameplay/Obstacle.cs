using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float playerSpeedAfterHit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            Debug.Log("Player got hit");

            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();

            if (playerMovement) 
            {
                playerMovement.ReduceCurrentSpeedByTime(playerSpeedAfterHit, 2);
            }
        }
    }
}
