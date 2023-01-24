using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    public float DangerZoneMaxDistanceFromPlayer;
    Coroutine DangerZoneCountdown;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            if (DangerZoneCountdown == null)
            {
                DangerZoneCountdown = StartCoroutine(StartDangerZoneCountdown(5));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopCoroutine(DangerZoneCountdown);
            DangerZoneCountdown = null;
        }
    }

    IEnumerator StartDangerZoneCountdown(float seconds) 
    {
        while (seconds > 0) 
        {
            Debug.Log("Loosing in: " + seconds);
            seconds -= 1;

            yield return new WaitForSeconds(1);
        }

        //We stayed more than the seconds allowed
        GameMode.Instance.EndGame();

        yield return null;
    }

    public void SetDeadZonePosition(Vector2 newPosition) 
    {
        transform.position = newPosition;
    }
}
