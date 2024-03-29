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

    [SerializeField] private Transform particleTrans;
    private float localX = -0.75f;

    private void Start()
    {
        GameMode.Instance.onGameStart += StartDangerZone;
        GameMode.Instance.onGameEnd += StopDangerZone;

        player = GameMode.Instance.GetPlayerController().transform;

        localX = particleTrans.localPosition.x;
    }

    public void StartDangerZone() 
    {
        isActive = true;
    }

    public void StopDangerZone() 
    {
        isActive = false;
    }

    private void LateUpdate()
    {
        if (PauseController.Instance.isPaused) { return; }

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

        if (particleTrans.position.x > Camera.main.transform.position.x + 15f) {
            particleTrans.position = new Vector3(Camera.main.transform.position.x + 15f, particleTrans.position.y, particleTrans.position.z);
        } else {
            particleTrans.localPosition = new Vector3(localX, particleTrans.localPosition.y, particleTrans.localPosition.z);
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

        OnGameEnd();


        yield return null;
    }

    void OnGameEnd() 
    {
        StopDangerZone();
        player.gameObject.SetActive(false);

        if (GameMode.Instance != null)
        {
            GameMode.Instance.EndGame();
        }
    }

    public void SetDeadZonePosition(Vector2 newPosition) 
    {
        transform.position = newPosition;
    }
}
