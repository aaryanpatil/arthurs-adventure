using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] float waitTime = 2f;
    Animator animator;
    void Awake() 
    {
        animator = GetComponentInChildren<Animator>();  
    }
    public void MakeTransition()
    {
        StartCoroutine(DelayPlayingAnimation());
    }

    IEnumerator DelayPlayingAnimation()
    {
        yield return new WaitForSecondsRealtime(waitTime);
        animator.SetTrigger("Start");
    }
}
