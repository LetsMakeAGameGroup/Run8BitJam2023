using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
    private AudioSource audioSource;
    bool wantsToJump;

    public bool isOnDoor;

    [SerializeField]private float groundedCooldown = 0.25f;
    private float groundedTimer = 0f;

    [SerializeField] private AudioClip runningClip;
    [SerializeField] private AudioClip runningOnFireClip;
    [SerializeField] private AudioClip[] jumpClips;
    [SerializeField] private AudioClip jetpackStartClip;
    [SerializeField] private AudioClip jetpackEndClip;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = (PlayerPrefs.HasKey("FXVolume") ? PlayerPrefs.GetFloat("FXVolume") / 100f : 0.5f);
    }

    private void Start() {
        currentSpeed = walkingSpeed;
    }

    private void Update() {
        if (groundedTimer <= 0) {
            // Raycast below the player to check if there is an object with groundLayer.
            isGrounded = Physics2D.Raycast(transform.position, -Vector2.up, 0.6f, groundLayer);
        } else {
            isGrounded = false;
            groundedTimer -= Time.deltaTime;
        }
        
        if (GameMode.Instance.gameState == GameState.InProcess && isGrounded && audioSource.clip != runningClip && audioSource.clip != runningOnFireClip) {
            audioSource.clip = (playerController.IsOnFire() ? runningOnFireClip : runningClip);
            audioSource.loop = true;
            audioSource.Play();
        }

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
                groundedTimer = groundedCooldown;

                audioSource.clip = jumpClips[Random.Range(0, jumpClips.Length)];
                audioSource.loop = false;
                audioSource.Play();
            } else if (playerController.batteries > 0) {
                playerController.batteries--;
                HUDManager.Instance.UpdateBatteries(playerController.batteries);
                jetpackParticles.Play();
                isJetpacking = true;
                currentSpeed += jetpackBoost;

                audioSource.clip = jetpackStartClip;
                audioSource.loop = true;
                audioSource.Play();
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

            audioSource.clip = jetpackEndClip;
            audioSource.loop = false;
            audioSource.Play();
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

    public float GetVelocityX() 
    {
        return rb.velocity.x;
    }

}
