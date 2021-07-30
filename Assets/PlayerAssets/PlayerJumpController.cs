using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 8f;
    public float jumpExtensionDuration = .5f;
    
    [Header("Layers")]
    public LayerMask platformsLayer;
    public LayerMask ledgesLayer;
    
    // status trackers
    private float _jumpExtensionTimer;
    
    // references
    private Rigidbody2D _rb;
    private Vector2 _size;
    private PlayerMovementController _movementController;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _size = GetComponent<BoxCollider2D>().bounds.size;
        _movementController = GetComponent<PlayerMovementController>();
    }
    
    /// <summary>
    /// Reset jump boost timer and change player state.
    /// </summary>
    public void InitiateJump()
    {
        PlayerState.Instance.activityState = PlayerState.ActivityState.Jumping;
        _jumpExtensionTimer = jumpExtensionDuration;
    }

    /// <summary>
    /// Handle jump boost and listen to transition to Falling.
    /// </summary>
    public void HandleUpdate(PlayerInputController.InputDesires input)
    {
        _movementController.MoveHorizontally(input.movementInput.x, input.isRunHeld);
        
        // stop boosting if jump is released
        if (!input.isJumpHeld)
            _jumpExtensionTimer = 0;
        
        // apply boost
        else if (_jumpExtensionTimer > 0)
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        
        // decrease jump extenstion timer and check if timer has run out
        _jumpExtensionTimer -= Time.deltaTime;
        if (_jumpExtensionTimer <= 0)
            PlayerState.Instance.activityState = PlayerState.ActivityState.Falling;
        
        // check if we're bonking into a platform
        CheckForHittingPlatformFromBelow();
    }

    /// <summary>
    /// If we hit a platform from below, set velocity to 0 immediately.
    /// </summary>
    private void CheckForHittingPlatformFromBelow()
    {
        Vector2 head = (Vector2) transform.position + Vector2.up * (_size.y * .5f);
        var pointA = head - new Vector2(_size.x * .25f, .1f);
        var pointB = head + new Vector2(_size.x * .25f, 0);
        
        // drop velocity and enter freefall
        if (Physics2D.OverlapArea(pointA, pointB, platformsLayer.value & ledgesLayer.value))
        {
            _jumpExtensionTimer = 0;
            var newYVelocity = Mathf.Min(_rb.velocity.y, 0);
            _rb.velocity = new Vector2(_rb.velocity.x, newYVelocity);
            PlayerState.Instance.activityState = PlayerState.ActivityState.Falling;
        }
    }
}
