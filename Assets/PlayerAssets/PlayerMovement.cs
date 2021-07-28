using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float movementSmoothing = 4f;
    public float vineMoveSpeed = 2.5f;
    
    [Header("Jump Settings")]
    public float jumpForce = 8f;
    public float jumpExtensionDuration = .5f;
    public float baseGravityScale = 1.5f;
    public float freeFallGravityScale = 3f;

    [Header("Layers")]
    public LayerMask platformsLayer;
    public LayerMask ledgesLayer;
    public LayerMask grassLayer;

    [Header("References")] 
    public GameObject hopShadow;
    public TilemapCollider2D ledgesCollider;
    public SpriteRenderer spriteRenderer;
    
    // references
    private Rigidbody2D _rb;
    private Vector2 _velocity;
    private Vector2 _size;
    
    // trackers
    private float _jumpExtensionTimer;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _size = GetComponent<BoxCollider2D>().bounds.size;
    }

    void Update()
    {
        // jump and vines
        HandleJump();
        HandleVineClimb();

        // d-pad movement
        if (PlayerState.Instance.state == ActivityState.Climbing)
            HandleVineMovement();
        else
            HandleHorizontalMovement();

        // special case checkers
        CheckForHittingPlatformFromBelow();
    }

    private void HandleVineClimb()
    {
        if (PlayerState.Instance.state != ActivityState.Climbing && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && IsTouchingGrass())
        {
            ChangeJumpState(ActivityState.Standing);
            _rb.gravityScale = 0;
            _rb.velocity = Vector2.zero;
            _jumpExtensionTimer = 0;
            PlayerState.Instance.state = ActivityState.Climbing;
        }
        else if (PlayerState.Instance.state == ActivityState.Climbing && !IsTouchingGrass())
        {
            ChangeJumpState(ActivityState.Falling);
        }
    }

    /// <summary>
    /// Move left and right smoothly.
    /// </summary>
    private void HandleHorizontalMovement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var targetVelocity = new Vector2(horizontalInput * moveSpeed, _rb.velocity.y);
        _rb.velocity = Vector2.SmoothDamp(_rb.velocity, targetVelocity, ref _velocity, movementSmoothing);
        
        // update sprite
        if (horizontalInput == 0)
        {
            if (IsInGroundedState())
                PlayerState.Instance.state = ActivityState.Standing;
        }
        else
        {
            if (IsInGroundedState())
                PlayerState.Instance.state = ActivityState.Walking;
            spriteRenderer.flipX = targetVelocity.x < 0;
        }
        
    }

    private void HandleVineMovement()
    {
        var inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        var vineMovement = inputDirection * (Time.deltaTime * vineMoveSpeed);
        _rb.MovePosition((Vector2)transform.position + vineMovement);

        PlayerState.Instance.isMovingOnVines = inputDirection != Vector2.zero;
    }

    /// <summary>
    /// All jump logic -- gravity, thrust, jump state, etc.
    /// </summary>
    private void HandleJump()
    {
        // initiate jump
        if (Input.GetKeyDown(KeyCode.Space) && (IsInGroundedState() || PlayerState.Instance.state == ActivityState.Climbing))
        {
            ChangeJumpState(ActivityState.Jumping);
        }
        
        // continue jump thrust
        if (PlayerState.Instance.state == ActivityState.Jumping)
        {
            if (Input.GetKey(KeyCode.Space) && _jumpExtensionTimer < jumpExtensionDuration)
            {
                // jump!
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                _jumpExtensionTimer += Time.deltaTime;
            }
            else
            {
                // if space was released (or extension expires), don't allow continuation of jump
                ChangeJumpState(ActivityState.Falling);
            }
        }

        // check if we've landed from a freefall
        if (PlayerState.Instance.state == ActivityState.Falling && IsGrounded())
        {
            _jumpExtensionTimer = 0;
            hopShadow.SetActive(false);
            ChangeJumpState(ActivityState.Standing);
        }
        
        // don't allow jumps after walking off a platform
        if (IsInGroundedState() && !IsGrounded())
        {
            ChangeJumpState(ActivityState.Falling);
        }
        
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
    private bool IsGrounded()
    {
        // check if we're grounded
        Vector2 feet = (Vector2) transform.position + Vector2.down * (_size.y * .5f);
        var pointA = feet - new Vector2(_size.x * .5f, 0);
        var pointB = feet + new Vector2(_size.x * .5f, -.1f);
        return Physics2D.OverlapArea(pointA, pointB, platformsLayer.value);
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

    private bool IsInGroundedState()
    {
        var groundedStates = new[]
        {
            ActivityState.Standing,
            ActivityState.Walking,
            ActivityState.Running
        };
        return groundedStates.Contains(PlayerState.Instance.state);
    }

    private bool IsTouchingGrass()
    {
        return Physics2D.OverlapCircle(transform.position, _size.y * .5f, grassLayer.value);
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
            ChangeJumpState(ActivityState.Falling);
        }
    }

    #region Change States

    /// <summary>
    /// Changes JumpState into the given state and adjust gravity.
    /// </summary>
    private void ChangeJumpState(ActivityState newJumpState)
    {
        if (newJumpState == ActivityState.Falling)
            _rb.gravityScale = freeFallGravityScale;
        else
            _rb.gravityScale = baseGravityScale;
        PlayerState.Instance.state = newJumpState;
    }
    
    #endregion
}
