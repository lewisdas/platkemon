using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    public ExtensionMethods.CardinalDirection facingDirection = ExtensionMethods.CardinalDirection.South;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") && GetCollisionDirection(other) == facingDirection)
            Debug.Log("jump");
    }

    /// <summary>
    /// Get direction from player to ledge.
    /// </summary>
    private ExtensionMethods.CardinalDirection GetCollisionDirection(Collision2D other)
    {
        var contactDirection = other.transform.position - transform.position;
        return ExtensionMethods.SnapToCardinalDirection(contactDirection);
    }
}
