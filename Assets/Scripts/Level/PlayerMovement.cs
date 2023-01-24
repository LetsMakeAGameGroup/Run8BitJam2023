using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkingSpeed = 10;
    [SerializeField] private float runningSpeed = 15;
    [SerializeField] private float jumpForce = 20;

    private bool isGrounded = false;

    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update(){
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runningSpeed : walkingSpeed;

        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);

        isGrounded = Physics2D.Raycast(transform.position, -Vector2.up, 0.6f, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
