using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    public float DangerZoneSpeed;
    public float SecondsToDestroyPlayer;
    Coroutine DangerZoneCountdown;
    bool isActive;
    public float maxDistanceFromPlayer;

    public Transform player;
    private void Start()
    {
        GameMode.Instance.onGameStart += StartDangerZone;
        GameMode.Instance.onGameEnd += StopDangerZone;

        player = GameMode.Instance.GetPlayerController().transform;
    }

    void StartDangerZone() 
    {
        isActive = true;
    }

    void StopDangerZone() 
    {
        isActive = false;
    }

    private void LateUpdate()
    {
        if (!isActive) 
        {
            return;
        }

        float playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerDistance > maxDistanceFromPlayer)
        {
            transform.Translate(Vector3.right * DangerZoneSpeed * 3 * Time.deltaTime);
        }
        else 
        {
            transform.Translate(Vector3.right * DangerZoneSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            if (DangerZoneCountdown == null)
            {
                DangerZoneCountdown = StartCoroutine(StartDangerZoneCountdown(SecondsToDestroyPlayer));
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
        //We die
        player.gameObject.SetActive(false);
        GameMode.Instance.EndGame();

        yield return null;
    }

    public void SetDeadZonePosition(Vector2 newPosition) 
    {
        transform.position = newPosition;
    }
}
