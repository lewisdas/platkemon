using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public float smoothTime = .3f;
    public float yDiffBeforeMove = 3f;

    private Vector3 _velocity = Vector3.zero;
    private bool _isTrackingY;

    void FixedUpdate()
    {
        // don't follow player if we're obeying a script
        if (!CameraController.Instance.isFollowingPlayer)
            return;
        
        // determine if we're tracking the target Y position or not
        var yDiff = Mathf.Abs(target.position.y - transform.position.y);
        if (!_isTrackingY && yDiff > yDiffBeforeMove)
            _isTrackingY = true;
        else if (_isTrackingY && yDiff < .1f)
            _isTrackingY = false;
        
        // find target camera position
        var targetY = (_isTrackingY) ? target.position.y : transform.position.y;
        var targetPos = new Vector3(target.position.x, targetY, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, 
            ref _velocity, smoothTime);
    }
}
