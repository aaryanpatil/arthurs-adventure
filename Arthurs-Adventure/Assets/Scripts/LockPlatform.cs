using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPlatform : MonoBehaviour
{
    [SerializeField] int keysCollected = 0;
    [SerializeField] Sprite lockedPlatform;
    [SerializeField] Sprite halfLockedPlatform;
    [SerializeField] Sprite unlockedPlatform;

    private void Awake() 
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
    public void ProcessKeyCollect(int n)
    {
        keysCollected++;
        if(keysCollected == 1)
        {
            GetComponent<SpriteRenderer>().sprite = halfLockedPlatform;
            return;
        }
        else if (keysCollected == 2)
        {
            GetComponent<SpriteRenderer>().sprite = unlockedPlatform;
            GetComponent<BoxCollider2D>().enabled = true;
        }      
    }


}
