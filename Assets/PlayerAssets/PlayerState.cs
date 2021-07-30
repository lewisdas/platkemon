using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance;

    public ControlState controlState = ControlState.FreeMove;
    public ActivityState activityState = ActivityState.Falling;

    public enum ControlState
    {
        FreeMove,
        AtTheWhimOfGod,
        Frozen
    }

    public enum ActivityState
    {
        Grounded,
        Jumping,
        Falling,
        Climbing
    }
    
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
}