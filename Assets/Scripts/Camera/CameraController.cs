using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public bool isFollowingPlayer = true;
    public float smoothTime = .5f;

    private Vector3 targetPos;
    private Vector3 _velocity;
    
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void FocusOnTarget(Vector3 target)
    {
        // don't go for target's z position
        target.z = transform.position.z;
        targetPos = target;

        isFollowingPlayer = false;
    }

    public void ReturnToPlayer()
    {
        isFollowingPlayer = true;
    }

    private void Update()
    {
        if (!isFollowingPlayer)
        {
        
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, 
                ref _velocity, smoothTime);
        }
    }
    
    
}
