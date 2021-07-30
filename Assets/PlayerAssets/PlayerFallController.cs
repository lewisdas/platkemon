using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallController : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask platformsLayer;

    private PlayerMovementController _movementController;
    private Vector2 _size;
    
    void Start()
    {
        _movementController = GetComponent<PlayerMovementController>();
        _size = GetComponent<BoxCollider2D>().bounds.size;
    }

    /// <summary>
    /// Allow for horizontal movement and check if player lands.
    /// </summary>
    public void HandleUpdate(PlayerInputController.InputDesires input)
    {
        _movementController.MoveHorizontally(input.movementInput.x, input.isRunHeld);
        if (IsGrounded())
            PlayerState.Instance.activityState = PlayerState.ActivityState.Grounded;
    }

    /// <summary>
    /// Check if player has landed on solid ground.
    /// </summary>
    private bool IsGrounded()
    {
        Vector2 feet = (Vector2) transform.position + Vector2.down * (_size.y * .5f);
        var pointA = feet - new Vector2(_size.x * .5f, 0);
        var pointB = feet + new Vector2(_size.x * .5f, -.1f);
        return Physics2D.OverlapArea(pointA, pointB, platformsLayer.value);
    }
}
