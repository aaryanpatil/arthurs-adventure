using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float waitTime = 0.5f;
    [SerializeField] PlatformMovementHorizontal platformHorizontal;
    [SerializeField] PlatformMovementVertical platformVertical;
    PlayerMovement player;
    Animator padAnimator;

    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        padAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        FindPlatform();
    }

    private void FindPlatform()
    {
        if (platformHorizontal != null)
        {
            gameObject.transform.parent = platformHorizontal.transform;
        }
        else if (platformVertical != null)
        {
            gameObject.transform.parent = platformVertical.transform;
        }
        else
        {
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player") 
        {
            player.BouncePadJump(jumpHeight);
            if (player.HasVertVelocity())
            {    
                padAnimator.SetBool("IsBouncing", true);
                FindObjectOfType<AudioManager>().Play("Bounce Pad", 0f);
            } 
            StartCoroutine(WaitForAnimation());
        }

        IEnumerator WaitForAnimation()
        {
            yield return new WaitForSecondsRealtime(waitTime);
            padAnimator.SetBool("IsBouncing", false);
        }
    }

    private void OnCollisionExit2D(Collision2D other) 
    {
        player.jumpCount = 1;
    }


}
