using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour {
    [SerializeField] private int health = 10;
    public bool isTriggered = false;
    Animator animator;
    public Collider2D doorCollider;

    PlayerController playerPunching;
    bool destroyed;

    [SerializeField] private AudioClip punchBreakDoorAudio;
    [SerializeField] private AudioClip fireBreakDoorAudio;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (isTriggered && Input.GetButtonDown("Submit")) health--;

        if (health <= 0) DestroyDoor();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (!playerController) return;

        if (playerController.IsOnFire()) {
            DestroyDoor();

            return;
        }

        isTriggered = true;
        playerPunching = playerController;
        playerPunching.canPunchDoor = true;
        playerPunching.SetOnDoor(true);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (!playerController) return;

        isTriggered = false;
    }

    private void DestroyDoor() {

        if(destroyed) return;

        if (playerPunching != null)
        {
            playerPunching.canPunchDoor = false;
            playerPunching.SetOnDoor(false);
        }

        animator.SetTrigger("BreakDoor");
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = (playerPunching != null ? punchBreakDoorAudio : fireBreakDoorAudio);
        audio.volume = (PlayerPrefs.HasKey("FXVolume") ? PlayerPrefs.GetFloat("FXVolume") / 100f : 0.5f);
        audio.Play();
        doorCollider.isTrigger = true;
        destroyed = true;
        //StartCoroutine();
    }

    IEnumerator DisableObject(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
        yield return null;
    }

    private void OnDisable()
    {
        destroyed = false;
        doorCollider.isTrigger = false;
        playerPunching = null;
    }

}
