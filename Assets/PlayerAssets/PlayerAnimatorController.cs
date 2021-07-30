using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer spriteRenderer;

    private Rigidbody2D _rb;
    private PlayerState.ActivityState _prevState;

    private void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
    }

    /// <summary>
    /// Update animator and flip sprite direction as needed.
    /// </summary>
    public void HandleUpdate(PlayerInputController.InputDesires input)
    {
        // check for state changes
        var state = PlayerState.Instance.activityState;
        var changedStateThisFrame = state != _prevState;
        _prevState = state;
        
        // set animation properties
        anim.SetFloat("yVelocity", _rb.velocity.y);
        anim.SetFloat("xVelocityAbs", Mathf.Abs(_rb.velocity.x));
        
        anim.SetBool("isGrounded", state == PlayerState.ActivityState.Grounded);
        anim.SetBool("isPressingDPad", input.movementInput != Vector2.zero);
        
        if (changedStateThisFrame && state == PlayerState.ActivityState.Climbing)
            anim.SetTrigger("isClimbing");
        
        // flip sprite
        if (input.movementInput != Vector2.zero)
            spriteRenderer.flipX = input.movementInput.x < 0;
    }
}
