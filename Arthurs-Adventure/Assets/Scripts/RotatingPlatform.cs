using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class RotatingPlatform : MonoBehaviour
{
    Rigidbody2D rb2d;
    Vector3 currentRotation;
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] float waitTime = 2f;

    private bool facingUp = true;
    private bool playerOnPlatform = false;

    private void Update()
    {
        if(playerOnPlatform)
        {
            RotateIntermittently();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Player")
        {
            StartCoroutine(DelayRotate());
        }
    }

    IEnumerator DelayRotate()
    {
        yield return new WaitForSecondsRealtime(waitTime);
        playerOnPlatform = true;
    }

    private void RotateIntermittently()
    {
        transform.Rotate(new Vector3(0, 0, -1) * rotationSpeed * Time.deltaTime);
    }
    
    private void OnCollisionExit2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Player")
        {
            playerOnPlatform = false;
        }   
    }
}
