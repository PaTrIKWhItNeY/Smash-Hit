/*
 * GAMEOBJECT: Player
 * REQUIRED DEPENDENCIES: Rigidbody2D
 * DESCRIPTION: Applies a constant downward acceleration to a Rigidbody2D and checks whether the object is currently touching the ground.
 * VERSION 1.0: Initial implementation of custom constant downward gravity and ground detection via OverlapCircle.
 */

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomGravity2D : MonoBehaviour
{
    [Header("Gravity Settings")]
    // DATA REQUIRED: Downward acceleration rate in units per second squared (e.g., 25.0)
    [SerializeField] private float gravityAcceleration = 25f;

    [Header("Ground Detection Settings")]
    // DATA REQUIRED: Child Transform positioned at the bottom/feet of the player collider
    [SerializeField] private Transform groundCheckPoint;

    // DATA REQUIRED: Radius of the circular ground detection area (e.g., 0.15)
    [SerializeField] private float groundCheckRadius = 0.15f;

    // DATA REQUIRED: LayerMask specifying which layer(s) count as walkable ground
    [SerializeField] private LayerMask groundLayer;

    // Exposes whether the player is currently touching the ground
    public bool IsGrounded { get; private set; }

    // Cached reference to the attached Rigidbody2D component
    private Rigidbody2D rb;

    private void Awake()
    {
        // Cache the Rigidbody2D and disable Unity's built-in gravity so custom gravity has full control
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    private void Update()
    {
        // Update method only calls dedicated ground check method
        CheckGroundStatus();
    }

    private void FixedUpdate()
    {
        // Physics velocity updates run in FixedUpdate for consistent frame-rate independent physics
        ApplyConstantGravity();
    }

    private void CheckGroundStatus()
    {
        // Prevent null reference errors if groundCheckPoint is not assigned yet
        if (groundCheckPoint == null)
        {
            IsGrounded = false;
            return;
        }

        // Check if a circle at the player's feet overlaps with any collider on the groundLayer
        IsGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    private void ApplyConstantGravity()
    {
        // Stop accumulating downward velocity if grounded and not moving upward
        if (IsGrounded && rb.linearVelocity.y <= 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            return;
        }

        // Apply constant downward acceleration: v = v0 - g * dt
        float newVerticalVelocity = rb.linearVelocity.y - (gravityAcceleration * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, newVerticalVelocity);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a visual circle in the Scene view to help adjust groundCheckPoint and groundCheckRadius
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}
