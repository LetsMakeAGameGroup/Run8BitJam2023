using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour {
    public float currentSpeed;
    [SerializeField] private float walkingSpeed = 10;
    [SerializeField] private float runningSpeed = 15;
    [SerializeField] private float jumpForce = 20;
    [SerializeField] private float jetpackBoost = 5;

    private bool isGrounded = false;
    [HideInInspector] public bool canMove = false;
    private bool isJetpacking = false;

    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private PlayerController playerController;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        currentSpeed = walkingSpeed;
    }

    private void Update() {
        if (!canMove) return;

        // Sets the player's speed to running or walking speed depending if the player is holding down the Sprint button or not.
        //bool isRunning = Input.GetButton("Sprint");
        //float currentSpeed = isRunning ? runningSpeed : walkingSpeed;
        if (playerController.fireTicks > 0) currentSpeed = runningSpeed;
        else currentSpeed = walkingSpeed;
        if (isJetpacking) currentSpeed += jetpackBoost;

        // Constantly move towards the right.
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);

        // Raycast below the player to check if there is an object with groundLayer.
        isGrounded = Physics2D.Raycast(transform.position, -Vector2.up, 0.6f, groundLayer);

        // When pressing the Jump button, do a normal jump when touching the ground. While in the air, prepare to jetpack when the player has at least one battery.
        if (Input.GetButtonDown("Jump")) {
            if (isGrounded) {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            } else if (playerController.batteries > 0) {
                isJetpacking = true;
            }
        }

        if (isJetpacking) Jetpack();
    }

    // Controls how jetpacking works once allowed to by isJetpacking.
    // TODO: Would recommend setting a time limit for how long they can jetpack for per battery use.
    private void Jetpack() {
        // Stop jetpacking once the player has touched the ground
        if (isGrounded) {
            isJetpacking = false;
            playerController.batteries--;
            HUDManager.Instance.UpdateBatteries(playerController.batteries);
            return;
        }

        if (Input.GetButton("Jump")) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void ReduceCurrentSpeedByTime(float newSpeed, float seconds)
    {
        StartCoroutine(ReduceCurrentSpeedByTimeC(newSpeed, seconds));
    }

    IEnumerator ReduceCurrentSpeedByTimeC(float newSpeed, float seconds) 
    {
        currentSpeed = newSpeed;

        yield return new WaitForSeconds(seconds);

        currentSpeed = walkingSpeed;

        yield return null;
    }
}
