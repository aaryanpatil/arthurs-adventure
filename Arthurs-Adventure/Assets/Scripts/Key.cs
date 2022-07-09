using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] AudioClip keyPickSFX;
    int keysCollected = 1;
    bool wasCollected = false;
    
    private float destroyDelay = 2f;
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<LockPlatform>().ProcessKeyCollect(keysCollected);
            FindObjectOfType<AudioManager>().Play("Coin Collect", 0f);
            gameObject.SetActive(false);
            Destroy(gameObject, destroyDelay);
        }
    }
}
