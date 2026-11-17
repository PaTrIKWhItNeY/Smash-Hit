/*
 * GAMEOBJECT: Player
 * REQUIRED DEPENDENCIES: Unity Input System Package (com.unity.inputsystem)
 * DESCRIPTION: Reads horizontal movement (A/D) and jump (Space) inputs using Unity's New Input System package and exposes them to other components.
 * VERSION 1.0: Initial implementation of 2D sidescroller input handling using code-defined InputActions for A, D, and Space.
 */

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput2D : MonoBehaviour
{
    // Stores the current horizontal input value (-1 for A, 1 for D, 0 for none)
    public float HorizontalInput { get; private set; }

    // Stores whether the jump button (Space) was pressed during the current frame
    public bool JumpPressed { get; private set; }

    // InputAction responsible for reading 1D horizontal movement from A and D keys
    private InputAction moveAction;

    // InputAction responsible for reading the jump button press from the Space key
    private InputAction jumpAction;

    private void Awake()
    {
        // Initialize the actions when the object is created
        SetupInputActions();
    }

    private void OnEnable()
    {
        // Enable input actions when the component becomes active
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        // Disable input actions when the component is disabled to prevent memory leaks
        moveAction.Disable();
        jumpAction.Disable();
    }

    private void Update()
    {
        // Update method only calls dedicated input reading methods
        ReadMovementInput();
        ReadJumpInput();
    }

    private void SetupInputActions()
    {
        // Create a 1D Axis composite binding where 'A' is negative (-1) and 'D' is positive (+1)
        moveAction = new InputAction(name: "Move", type: InputActionType.Value);
        moveAction.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/a")
            .With("Positive", "<Keyboard>/d");

        // Create a button action bound directly to the Spacebar
        jumpAction = new InputAction(name: "Jump", type: InputActionType.Button, binding: "<Keyboard>/space");
    }

    private void ReadMovementInput()
    {
        // Read the current float value (-1, 0, or 1) from the move action
        HorizontalInput = moveAction.ReadValue<float>();
    }

    private void ReadJumpInput()
    {
        // Check if the Space key was pressed down on this exact frame
        JumpPressed = jumpAction.WasPressedThisFrame();
    }
}