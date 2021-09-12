using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimatorController : MonoBehaviour
{
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void StopWalking()
    {
        _anim.SetBool("isMoving", false);
    }

    public void SetWalkDirection(Vector2 direction)
    {
        _anim.SetBool("isMoving", true);
        _anim.SetFloat("moveX", direction.x);
        _anim.SetFloat("moveY", direction.y);
    }

    public void SetDirection(Vector2 direction)
    {
        _anim.SetFloat("moveX", direction.x);
        _anim.SetFloat("moveY", direction.y);
    }
}
