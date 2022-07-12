using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using System.Runtime.InteropServices;
using Cinemachine;


public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    [Header("Player Death")] 
    [SerializeField] Vector2 deathKick = new Vector2(-10f, 5f);
    [SerializeField] float deathGravity = 100f;
    [SerializeField] float deathDrag = 5f;

    [Header("Firing")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] int pointsForBulletFire = 100;

    [Header("Platform Movement")]
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask bouncingLayer;

    // [Header("Camera")]
    // [SerializeField] private CinemachineVirtualCamera leftCam;
    

    Vector2 moveInput;
    Rigidbody2D myRigidBody; 
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    CircleCollider2D circleCollider2D;
    Animator myAnimator;
    ParticleSystem jumpParticle;
    
    private Transform playerTransform;
    private Transform platform;

    float gravityScaleAtStart;
    bool isAlive = true;
    bool isPaused = false;
    bool isBouncing = false;
    public int jumpCount;

    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    [SerializeField] private float smoothInputSpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        playerTransform = GetComponent<Transform>();
        jumpParticle = GetComponent<ParticleSystem>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        jumpCount = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isAlive || PauseMenu.isPaused) { return; }

        Run();
        FlipSprite();
        ClimbLadder();
        CancelJumpAnimation();
        Die();
        //CheckBounce();
        CheckBouncePad();
        PlatformMovement();       
        HasVertVelocity();
        
        if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            jumpCount = 1;
        }
    }

    public bool HasVertVelocity()
    {
        return myRigidBody.velocity.y > 0;
    } 

    bool CheckBouncePad()
    {
        float extraRayLength = 0.5f;
        float heightAdjustment = 0.2f;
        RaycastHit2D raycastHit = Physics2D.Raycast(myBodyCollider.bounds.center - new Vector3(0, heightAdjustment, 0), Vector2.right, myBodyCollider.bounds.extents.x + extraRayLength, bouncingLayer);

        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;      
        } 
        else
        {   
            rayColor = Color.red;            
        }
        Debug.DrawRay(myBodyCollider.bounds.center - new Vector3(0, heightAdjustment, 0), Vector2.right * (myBodyCollider.bounds.extents.x + extraRayLength), rayColor);

        return raycastHit.collider != null;
    }

    public void BouncePadJump(float jumpHeight)
    {
        isBouncing = true;
        myRigidBody.velocity += new Vector2(0f, jumpHeight);
        myAnimator.SetBool("IsJumping", true);
        FindObjectOfType<AudioManager>().Play("Bounce Pad", 0f);
    }
    
    private void PlatformMovement()
    {
        if(IsOnPlatform())
        {
            jumpCount = 1;
        }
        else
        {
            IsOnGround();
        }
    }

    private bool IsOnPlatform()
    {
        float extraRayHeight = 0.2f;
        
        RaycastHit2D raycastHit = Physics2D.Raycast(myFeetCollider.bounds.center, Vector2.down, myFeetCollider.bounds.extents.y + extraRayHeight, platformLayer);

        Color rayColor;
        if (raycastHit.collider != null)
        {

            rayColor = Color.green;
            myAnimator.SetBool("IsJumping", false);
            gameObject.transform.parent = raycastHit.collider.transform;
        } 
        else
        {   
            rayColor = Color.red;
            myAnimator.SetBool("IsJumping", true);
            gameObject.transform.parent = null;
        }
        Debug.DrawRay(myFeetCollider.bounds.center, Vector2.down * (myFeetCollider.bounds.extents.y + extraRayHeight), rayColor);

        return raycastHit.collider != null;
    }

    private bool IsOnGround()
    {
        
        float extraRayHeight = 0.5f;
        isBouncing = false;
        RaycastHit2D raycastHit = Physics2D.Raycast(myFeetCollider.bounds.center, Vector2.down, myFeetCollider.bounds.extents.y + extraRayHeight, groundLayer);

        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
    
            myAnimator.SetBool("IsJumping", false);  
            transform.parent = raycastHit.collider.transform;
        } 
        else
        {   
            rayColor = Color.red;
        }
        Debug.DrawRay(myFeetCollider.bounds.center, Vector2.down * (myFeetCollider.bounds.extents.y + extraRayHeight), rayColor);

        return raycastHit.collider != null;
    }

    private void CheckBounce()
    {
        if(!isAlive || PauseMenu.isPaused ) { return; }

        if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Bouncing")))
        {
            FindObjectOfType<AudioManager>().Play("Bounce Pad", 0f);
            jumpCount = 1;
        }

    }

    private void FlipSprite()
    {
        if(PauseMenu.isPaused) { return; }

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidBody.velocity.x), 1f);
            // if(Mathf.Sign(myRigidBody.velocity.x) == -1)
            // {
            //     leftCam.enabled = true;
            //     return;
            // }
            // leftCam.enabled = false;

        }
    }

    void OnFire(InputValue value)
    {
        if(!isAlive || PauseMenu.isPaused) { return; }

        // if(EventSystem.current.IsPointerOverGameObject()) { return; }

        if(FindObjectOfType<GameSession>().GetCoins() == 0) { return; }

        FindObjectOfType<AudioManager>().Play("Bullet Shoot", 0f);
        FindObjectOfType<GameSession>().ReduceScoreOnFire(pointsForBulletFire);
        if(transform.localScale.x == 1)
        {
            Instantiate(bullet, gun.position, Quaternion.Euler(new Vector3(0, 0, 90)));
        }
        else
        {
            Instantiate(bullet, gun.position, Quaternion.Euler(new Vector3(0, 0, -90)));
        }
        
    }
    void OnMove(InputValue value)
    {
        if(!isAlive || PauseMenu.isPaused) { return; }

        moveInput = value.Get<Vector2>();
        // Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if(!isAlive || PauseMenu.isPaused || isBouncing || CheckBouncePad()) { return; }

        if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing", "Moving")) || IsOnPlatform() || jumpCount > 0)
        {
            if(value.isPressed)
            {
                // if (GetVelocity().y < 0 && jumpCount == 1)
                // {
                //     myRigidBody.velocity += new Vector2(0f, jumpSpeed + 5);
                // }
                // else if (GetVelocity().y > 0 && jumpCount == 1)
                // {
                //     myRigidBody.velocity += new Vector2(0f, jumpSpeed - 2);
                // }
                // else
                // {
                    //myRigidBody.velocity += new Vector2(0f, jumpSpeed);
                // }
                // Debug.Log(jumpCount);
                
            
                
                if(myRigidBody.velocity.y < 0)
                {
                    myRigidBody.AddForce(new Vector2(0f, jumpSpeed - myRigidBody.velocity.y), ForceMode2D.Impulse);
                }
                else if(myRigidBody.velocity.y > 0)
                {
                    myRigidBody.AddForce(new Vector2(0f, jumpSpeed - myRigidBody.velocity.y), ForceMode2D.Impulse);
                }
                else
                {
                    myRigidBody.AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);
                }
                
                myAnimator.SetBool("IsJumping", true);
                jumpParticle.Play();
                FindObjectOfType<AudioManager>().Play("Player Jump", 0f);
                // jumpCount += 1; 
                jumpCount--;   
            }
            else
            {
                myAnimator.SetBool("IsJumping", false); 
            }
        }
    }

    void OnPauseMenu()
    {
        FindObjectOfType<PauseMenu>().DisplayPauseMenu();
    }

    void ClimbLadder()
    {
        Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, moveInput.y * climbSpeed);
        if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidBody.velocity = playerVelocity;
            myRigidBody.gravityScale = 0f;
            bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
            if(playerHasVerticalSpeed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
            {
                myAnimator.SetBool("IsClimbing", true);
            }
            else
            {
                myAnimator.SetBool("IsClimbing", false);
            }  
        }
        else
        {
            myAnimator.SetBool("IsClimbing", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
        }
    }
    void CancelJumpAnimation()
    {
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        if(!playerHasVerticalSpeed)
        {
            myAnimator.SetBool("IsJumping", false);
        }   
    }

    void Run()
    {
        currentInputVector = Vector2.SmoothDamp(currentInputVector, moveInput, ref smoothInputVelocity, smoothInputSpeed);
        //Debug.Log(currentInputVector);
        Vector2 playerVelocity = new Vector2(currentInputVector.x * runSpeed *  Time.fixedDeltaTime, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if(moveInput.x == 0)
        {
            myAnimator.SetBool("IsRunning", false);
        }
        else
        {
            myAnimator.SetBool("IsRunning", true);
        }
    }

    void Die()
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;

            myAnimator.SetTrigger("Dying");
            myAnimator.SetBool("IsClimbing", false);
            myAnimator.SetBool("IsJumping", false);
            myAnimator.SetBool("IsRunning", false);

            myRigidBody.velocity = deathKick;        
            myRigidBody.gravityScale = deathGravity;
            myRigidBody.drag = deathDrag;
            myBodyCollider.enabled = false;
            
            FindObjectOfType<AudioManager>().Play("Player Death", 0f);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
