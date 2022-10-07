using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles event from PlayerInputActions (native unity class).
/// Offers reading of properties that are immutable from the outside,
/// but mutable from the class itself. A good way to not bloat other classes with input management code.
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    /// <summary>
    /// Gets the current movement input from the player.
    /// Vector elements lays in the range [-1, 1].
    /// </summary>
    public Vector2 MoveInput { get; private set; }
    
    // PlayerInputActions is the C# class that Unity generated.
    // It encapsulates the data from the .inputactions asset we created
    // and automatically looks up all the maps and actions for us.
    private PlayerInputActions playerInputActions;

    private void OnEnable()
    {
        // Setup and enable the input action map.
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // Subscribe to when movement is performed and canceled.
        playerInputActions.Player.Move.performed += SetMove;
        playerInputActions.Player.Move.canceled += SetMove;
    }

    private void OnDisable()
    {
        // Unsubscribe to when movement is performed and canceled.
        // Do this to prevent memory leaks, since the publishes would still
        // hold a reference to this object and thereby prevent the garbage collector
        // from clearing the memory for this object.
        playerInputActions.Player.Move.performed -= SetMove;
        playerInputActions.Player.Move.canceled -= SetMove;
        
        // Disable the input action map.
        playerInputActions.Player.Disable();
    }

    /// <summary>
    /// Gets called each time a move action is performed or canceled.
    /// </summary>
    /// <param name="context">information provided to action callbacks about what triggered an action.</param>
    private void SetMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }
}