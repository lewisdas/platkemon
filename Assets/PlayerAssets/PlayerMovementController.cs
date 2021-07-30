using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float runSpeed = 8f;
    public float movementSmoothing = 4f;

    [Header("References")] 
    public GameObject hopShadow;
    public TilemapCollider2D ledgesCollider;
    
    // references
    private Rigidbody2D _rb;
    private Vector2 _velocity;
    private Vector2 _size;

    // component references
    private PlayerJumpController _jumpController;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _size = GetComponent<BoxCollider2D>().bounds.size;
        
        // grab component references
        _jumpController = GetComponent<PlayerJumpController>();
    }

    /// <summary>
    /// Listen for jump, move input desires.
    /// </summary>
    public void HandleUpdate(PlayerInputController.InputDesires input)
    {
        if (input.movementInput.x != 0)
            MoveHorizontally(input.movementInput.x, input.isRunHeld);
        if (input.isJumpPressed)
            _jumpController.InitiateJump();
    }

    /// <summary>
    /// Add horizontal velocity.
    /// </summary>
    public void MoveHorizontally(float horizontalInput, bool isRunHeld)
    {
        var maxMoveSpeed = (isRunHeld) ? runSpeed : walkSpeed;
        var targetVelocity = new Vector2(horizontalInput * maxMoveSpeed, _rb.velocity.y);
        _rb.velocity = Vector2.SmoothDamp(_rb.velocity, targetVelocity, ref _velocity, movementSmoothing);
    }
    
    /*
    /// <summary>
    /// All jump logic -- gravity, thrust, jump state, etc.
    /// </summary>
    private void HandleJump()
    {
        // fall off ledge
        if (IsOnLedge() && !IsGrounded())
            StartCoroutine(FallOffLedge());
    }

    private IEnumerator FallOffLedge()
    {
        // little hop
        hopShadow.SetActive(true);
        ChangeJumpState(ActivityState.Falling);
        _rb.velocity = new Vector2(_rb.velocity.x, 8f);
        
        // fall
        ledgesCollider.enabled = false;
        
        // re-enable collider
        yield return new WaitForSeconds(.75f);
        hopShadow.SetActive(false);
        ledgesCollider.enabled = true;
    }

    /// <summary>
    /// Check if we're grounded via OverlapArea.
    /// </summary>
    private bool IsOnLedge()
    {
        // check if we're grounded
        Vector2 feet = (Vector2) transform.position + Vector2.down * (_size.y * .5f);
        var pointA = feet - new Vector2(_size.x * .5f, .3f);
        var pointB = feet + new Vector2(_size.x * .5f, .2f);
        return Physics2D.OverlapArea(pointA, pointB, ledgesLayer.value);
    }
    */
}
