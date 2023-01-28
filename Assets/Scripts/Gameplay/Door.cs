using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour {
    [SerializeField] private int health = 10;
    private bool isTriggered = false;
    Animator animator;
    public Collider2D doorCollider;

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
        } else {
            isTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (!playerController) return;

        isTriggered = false;
    }

    private void DestroyDoor() {
        animator.SetTrigger("BreakDoor");
        doorCollider.isTrigger = true;
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
        doorCollider.isTrigger = false;
    }

}
