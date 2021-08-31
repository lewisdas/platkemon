using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationBlock : MonoBehaviour
{
    [Header("Effects")]
    public GameObject objectToSpawn;
    
    [Header("Graphics")]
    public GameObject exclamationGraphics;
    public GameObject dullGraphics;

    private bool _isStruck;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!_isStruck && other.collider.CompareTag("Player"))
        {
            StartCoroutine(SpawnObject());

            _isStruck = true;
            exclamationGraphics.SetActive(false);
            dullGraphics.SetActive(true);
        }
    }

    /// <summary>
    /// Move camera to object, spawn it, and return to player.
    /// </summary>
    private IEnumerator SpawnObject()
    {
        // move to object spawn spot
        CameraController.Instance.FocusOnTarget(objectToSpawn.transform.position);
        yield return new WaitForSeconds(1f);
        
        // spawn object
        objectToSpawn.SetActive(true);
        yield return new WaitForSeconds(1f);
        
        // return to player
        CameraController.Instance.ReturnToPlayer();
        yield return new WaitForSeconds(1f);
    }
}
