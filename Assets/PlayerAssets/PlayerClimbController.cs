using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbController : MonoBehaviour
{
    public float vineMoveSpeed = 2.5f;
    
    [Header("Layers")]
    public LayerMask grassLayer;
    
    private float _initGravity;
    private Vector2 _size;
    private Rigidbody2D _rb;
    private PlayerJumpController _jumpController;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _size = GetComponent<BoxCollider2D>().bounds.size;
        _jumpController = GetComponent<PlayerJumpController>();
    }
    
    /// <summary>
    /// Turn off usual physics to begin climbing.
    /// </summary>
    public void InitiateClimbing()
    {
        _initGravity = _rb.gravityScale;
        _rb.gravityScale = 0;
        _rb.velocity = Vector2.zero;
        PlayerState.Instance.activityState = PlayerState.ActivityState.Climbing;
    }

    public void HandleUpdate(PlayerInputController.InputDesires input)
    {
        // move along vines
        if (IsTouchingGrass())
            Climb(input.movementInput);

        // listen for jump
        if (input.isJumpPressed)
        {
            _rb.gravityScale = _initGravity;
            _jumpController.InitiateJump();
        }
    }

    private void Climb(Vector2 movementDirection)
    {
        var vineMovement = movementDirection * (Time.deltaTime * vineMoveSpeed);
        _rb.MovePosition((Vector2)transform.position + vineMovement);
    }

    /// <summary>
    /// Checks if Player is touching grass layer.
    /// </summary>
    /// <returns></returns>
    public bool IsTouchingGrass()
    {
        return Physics2D.OverlapCircle(transform.position, 
            _size.y * .5f, grassLayer.value);
    }
}
