using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    public Animator anim;

    private Rigidbody2D _rb;
    private ActivityState _prevState;

    private void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        // check for state changes
        var state = PlayerState.Instance.state;
        var changedStateThisFrame = false;
        if (state != _prevState)
        {
            _prevState = state;
            changedStateThisFrame = true;
        }
        
        // handle animation changes
        anim.SetBool("isWalking", PlayerState.Instance.state == ActivityState.Walking);
        anim.SetFloat("yVelocity", _rb.velocity.y);
        anim.SetBool("isMovingOnVines", PlayerState.Instance.isMovingOnVines);
        if (changedStateThisFrame && state == ActivityState.Climbing)
            anim.SetTrigger("isClimbing");
    }
}
