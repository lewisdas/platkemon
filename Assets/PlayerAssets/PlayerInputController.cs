using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerInputController : MonoBehaviour
{
    
    // component references
    private PlayerMovementController _movementController;
    private PlayerJumpController _jumpController;
    private PlayerFallController _fallController;
    private PlayerClimbController _climbController;
    private PlayerAnimatorController _animatorController;
    
    // input trackers
    public struct InputDesires
    {
        public Vector2 movementInput;
        public bool isJumpPressed;
        public bool isJumpHeld;
        public bool isClimbHeld;
        public bool isRunHeld;
    }
    private InputDesires _inputDesires;

    private Rigidbody2D _rb;

    void Start()
    {
        // grab component references
        _movementController = GetComponent<PlayerMovementController>();
        _jumpController = GetComponent<PlayerJumpController>();
        _fallController = GetComponent<PlayerFallController>();
        _climbController = GetComponent<PlayerClimbController>();
        _animatorController = GetComponent<PlayerAnimatorController>();
        _rb = GetComponent<Rigidbody2D>();
     
        // initialize values
        _inputDesires = new InputDesires();
    }
    
    public void HandleUpdate()
    {
        if (PlayerState.Instance.controlState == PlayerState.ControlState.FreeMove)
        {
            // interpret user input desires
            ProcessInput();
            
            // handle state-specific updates
            if (PlayerState.Instance.activityState == PlayerState.ActivityState.Grounded)
                _movementController.HandleUpdate(_inputDesires);
            else if (PlayerState.Instance.activityState == PlayerState.ActivityState.Jumping)
                _jumpController.HandleUpdate(_inputDesires);
            else if (PlayerState.Instance.activityState == PlayerState.ActivityState.Falling)
                _fallController.HandleUpdate(_inputDesires);
            else if (PlayerState.Instance.activityState == PlayerState.ActivityState.Climbing)
                _climbController.HandleUpdate(_inputDesires);
            
            // handle state-agnostic checks
            CheckForStateChanges(_inputDesires);
        }
        
        // todo: ledge handling
        
        // update animation
        _animatorController.HandleUpdate(_inputDesires);
    }

    /// <summary>
    /// Store user input for all movement actions.
    /// </summary>
    private void ProcessInput()
    {
        _inputDesires.movementInput = new Vector2(Input.GetAxis("Horizontal"), 
            Input.GetAxis("Vertical")).normalized;
        _inputDesires.isJumpPressed = Input.GetKeyDown(KeyCode.Space);
        _inputDesires.isJumpHeld = Input.GetKey(KeyCode.Space);
        _inputDesires.isClimbHeld = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        _inputDesires.isRunHeld = Input.GetKey(KeyCode.LeftShift);
    }

    /// <summary>
    /// Check for state-agnostic state changes.
    /// </summary>
    private void CheckForStateChanges(InputDesires input)
    {
        // check if player is falling
        if (_rb.velocity.y < -.1f)
            PlayerState.Instance.activityState = PlayerState.ActivityState.Falling;

        // check if player can climb on grass
        if (input.isClimbHeld && _climbController.IsTouchingGrass())
            _climbController.InitiateClimbing();
    }
}
