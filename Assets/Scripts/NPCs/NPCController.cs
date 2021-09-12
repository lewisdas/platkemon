using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public float stepDuration;
    public event Action FinishedWalking;

    private NPCAnimatorController _anim;
    private Dictionary<Direction, Vector2> _directionVectors;
    private bool _isMoving;
    
    private void Awake()
    {
        _anim = GetComponentInChildren<NPCAnimatorController>();
        
        _directionVectors = new Dictionary<Direction, Vector2>
        {
            {Direction.Left, Vector2.left},
            {Direction.Right, Vector2.right},
            {Direction.Up, Vector2.up},
            {Direction.Down, Vector2.down}
        };

        FinishedWalking += () => Debug.Log("finished walking.");
    }

    void Start()
    {
        // just a test
        var steps = new Direction[]
        {
            Direction.Right,
            Direction.Right,
            Direction.Down,
            Direction.Left,
            Direction.Up
        };
        Move(steps);
    }

    public void Move(Direction[] steps, int currentStep = 0)
    {
        if (currentStep == steps.Length)
        {
            FinishedWalking();
            return;
        }
        var step = steps[currentStep];
        StartCoroutine(Step(step, () => Move(steps, ++currentStep)));
    }

    private IEnumerator Step(Direction direction, Action callback)
    {
        var startPos = transform.position;
        var endPos = transform.position + (Vector3)_directionVectors[direction];

        _anim.SetWalkDirection(_directionVectors[direction]);
        
        float t = 0;
        while (t < stepDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t / stepDuration);
            t += Time.deltaTime;
            yield return null;
        }
        
        _anim.StopWalking();
        transform.position = endPos;
        callback();
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    };
}
