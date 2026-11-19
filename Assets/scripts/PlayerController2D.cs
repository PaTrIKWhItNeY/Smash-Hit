/*
 * GAMEOBJECT: Player
 * REQUIRED DEPENDENCIES: Rigidbody2D, PlayerInput2D, CustomGravity2D
 * DESCRIPTION: Controls 2D horizontal movement and jumping by combining input data with ground status and applying velocity to the Rigidbody2D.
 * VERSION 1.0: Initial implementation of 2D sidescroller horizontal movement and grounded jumping.
 */

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput2D))]
[RequireComponent(typeof(CustomGravity2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    // DATA REQUIRED: Horizontal movement speed in units per second (e.g., 7.0)
    [SerializeField] private float moveSpeed = 7f;

    // DATA REQUIRED: Upward velocity applied instantly when jumping (e.g., 12.0)
    [SerializeField] private float jumpForce = 12f;

    // Cached component references
    private Rigidbody2D rb;
    private PlayerInput2D playerInput;
    private CustomGravity2D customGravity;

    // Flag to transfer jump input from Update to FixedUpdate cleanly
    private bool jumpRequested;

    private void Awake()
    {
        // Retrieve required components on the same GameObject
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput2D>();
        customGravity = GetComponent<CustomGravity2D>();

        // Freeze Z rotation so the 2D character does not tip over when moving
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Update method only calls input-buffering method
        BufferJumpInput();
    }

    private void FixedUpdate()
    {
        // Apply physics movement and jump execution in FixedUpdate
        ApplyHorizontalMovement();
        ExecuteJump();
    }

    private void BufferJumpInput()
    {
        // Record jump request if Space was pressed while standing on the ground
        if (playerInput.JumpPressed && customGravity.IsGrounded)
        {
            jumpRequested = true;
        }
    }

    private void ApplyHorizontalMovement()
    {
        // Set horizontal velocity based on A/D input while preserving current vertical velocity
        float targetVelocityX = playerInput.HorizontalInput * moveSpeed;
        rb.linearVelocity = new Vector2(targetVelocityX, rb.linearVelocity.y);
    }

    private void ExecuteJump()
    {
        // Apply upward jump velocity if a valid jump was requested
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }
}