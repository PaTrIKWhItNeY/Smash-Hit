/*
 * GAMEOBJECT: Player (or child visual GameObject with SpriteRenderer)
 * REQUIRED DEPENDENCIES: SpriteRenderer, Rigidbody2D, PlayerInput2D, CustomGravity2D
 * DESCRIPTION: Swaps the SpriteRenderer's active sprite between 4 Inspector-assigned sprites (Left, Right, Jump, Fall/Idle) based on movement and ground state.
 * VERSION 1.0: Initial implementation of 4-slot sprite swapping where Fall also serves as the Idle sprite.
 */

using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSwapper2D : MonoBehaviour
{
    [Header("Sprite Slots")]
    // DATA REQUIRED: Sprite displayed when moving left on the ground
    [SerializeField] private Sprite leftSprite;

    // DATA REQUIRED: Sprite displayed when moving right on the ground
    [SerializeField] private Sprite rightSprite;

    // DATA REQUIRED: Sprite displayed when airborne and moving upward
    [SerializeField] private Sprite jumpSprite;

    // DATA REQUIRED: Sprite displayed when falling OR when idle on the ground
    [SerializeField] private Sprite fallAndIdleSprite;

    [Header("Component References")]
    // DATA REQUIRED: Reference to the Player's Rigidbody2D component
    [SerializeField] private Rigidbody2D rb;

    // DATA REQUIRED: Reference to the Player's PlayerInput2D component
    [SerializeField] private PlayerInput2D playerInput;

    // DATA REQUIRED: Reference to the Player's CustomGravity2D component
    [SerializeField] private CustomGravity2D customGravity;

    // Cached reference to the SpriteRenderer on this GameObject
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Cache the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Update method only calls the sprite evaluation method
        UpdateSpriteState();
    }

    private void UpdateSpriteState()
    {
        // Priority 1: Check if the player is in the air
        if (!customGravity.IsGrounded)
        {
            // Moving upward uses Jump sprite; moving downward uses Fall/Idle sprite
            if (rb.linearVelocity.y > 0.01f)
            {
                SetSprite(jumpSprite);
            }
            else
            {
                SetSprite(fallAndIdleSprite);
            }
            return;
        }

        // Priority 2: Player is grounded; check horizontal movement direction
        if (playerInput.HorizontalInput < -0.01f)
        {
            SetSprite(leftSprite);
        }
        else if (playerInput.HorizontalInput > 0.01f)
        {
            SetSprite(rightSprite);
        }
        else
        {
            // Priority 3: Player is grounded and not moving horizontally (Idle uses Fall sprite)
            SetSprite(fallAndIdleSprite);
        }
    }

    private void SetSprite(Sprite targetSprite)
    {
        // Only assign the sprite if it is valid and differs from the currently active sprite
        if (targetSprite != null && spriteRenderer.sprite != targetSprite)
        {
            spriteRenderer.sprite = targetSprite;
        }
    }
}
