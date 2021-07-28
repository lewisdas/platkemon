using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance;

    public ActivityState state = ActivityState.Standing;
    public bool isMovingOnVines;
    
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
}

// enums
public enum ActivityState
{
    Standing,
    Walking,
    Running,
    Jumping,
    Falling,
    Climbing
};