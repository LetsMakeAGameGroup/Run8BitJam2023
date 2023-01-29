using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour {
    public float currentSpeed;
    public float walkingSpeed = 10;
    [SerializeField] private float runningSpeed = 15;
    [SerializeField] private float jumpForce = 20;
    [SerializeField] private float jetpackBoost = 5;
    [SerializeField] private float minSpeedThreshold = 2f;
    [SerializeField] private float maxSpeedThreshold = 18f;
    [SerializeField] private ParticleSystem jetpackParticles;

    private bool isGrounded = false;
    [HideInInspector] public bool canMove = false;
    private bool isJetpacking = false;

    private float slowedSpeedPerc = 0;
    [HideInInspector] public int slowCount = 0;

    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private PlayerController playerController;
    bool wantsToJump;

    public bool isOnDoor;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    private void Start() {
        currentSpeed = walkingSpeed;
    }

    private void Update() {

        // Raycast below the player to check if there is an object with groundLayer.
        isGrounded = Physics2D.Raycast(transform.position, -Vector2.up, 0.6f, groundLayer);

        if (PauseController.Instance.isPaused) { rb.isKinematic = true; rb.simulated = false; } else { rb.isKinematic = false; rb.simulated = true; }

        if (!canMove) return;

        // Sets the player's speed to runningSpeed or walkingSpeed depending if the player is on fire or not. Increases with jetpackBoost when jetpacking.
        if (slowCount == 0) {
            currentSpeed = (playerController.IsOnFire() ? runningSpeed : walkingSpeed);
            if (isJetpacking) currentSpeed += jetpackBoost;
            if (currentSpeed > maxSpeedThreshold) currentSpeed = maxSpeedThreshold;
        }

        //if (!isOnDoor)
        //{
            // Constantly move towards the right.
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        //}


        // When pressing the Jump button, do a normal jump when touching the ground. While in the air, prepare to jetpack when the player has at least one battery.
        if (Input.GetButtonDown("Jump")) {
            if (isGrounded) {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            } else if (playerController.batteries > 0) {
                playerController.batteries--;
                HUDManager.Instance.UpdateBatteries(playerController.batteries);
                jetpackParticles.Play();
                isJetpacking = true;
                currentSpeed += jetpackBoost;
            }
        }

        if (isJetpacking) Jetpack();
    }

    // Controls how jetpacking works once allowed to by isJetpacking.
    private void Jetpack() {
        if (Input.GetButton("Jump")) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        } else {
            isJetpacking = false;
            jetpackParticles.Stop();
            if (slowCount > 0) currentSpeed -= jetpackBoost;
        }
    }

    public void ReduceCurrentSpeedByTime(float newSlowPerc, float seconds) {
        StartCoroutine(ReduceCurrentSpeedByTimeC(newSlowPerc, seconds));
    }

    IEnumerator ReduceCurrentSpeedByTimeC(float newSlowPerc, float seconds) {
        slowedSpeedPerc = newSlowPerc;
        slowCount++;
        SetSlowedSpeed();

        yield return new WaitForSeconds(seconds);

        slowCount--;
        SetSlowedSpeed();
    }

    public void SetSlowedSpeed() {
        float newSlowedSpeed = walkingSpeed;
        for (int i = 0; i < slowCount; i++) {
            newSlowedSpeed *= (100f - slowedSpeedPerc) / 100f;
            if (newSlowedSpeed <= minSpeedThreshold) {
                newSlowedSpeed = minSpeedThreshold;
                break;
            }
        }
        currentSpeed = newSlowedSpeed;
    }

    public bool IsGrounded() 
    {
        return isGrounded;
    }


}
