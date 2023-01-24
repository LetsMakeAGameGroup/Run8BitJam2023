using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float walkingSpeed = 10;
    [SerializeField] private float runningSpeed = 15;
    [SerializeField] private float jumpForce = 20;

    private bool isGrounded = false;
    [HideInInspector] public bool canMove = true;
    private bool isJetpacking = false;

    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private PlayerController playerController;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update() {
        if (!canMove) return;

        // Sets the player's speed to running or walking speed depending if the player is holding down the Sprint button or not.
        bool isRunning = Input.GetButton("Sprint");
        float currentSpeed = isRunning ? runningSpeed : walkingSpeed;

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
}
