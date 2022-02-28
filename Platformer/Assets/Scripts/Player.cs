using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 5;
    [SerializeField] float jumpSpeed = 25;
    //[SerializeField] float bounceOffEnemyFactor = 40;
    //[SerializeField] AudioClip bounceSFX = null;
    [SerializeField] AnimationClip DeathAnimation = null;

    // State
    bool isAlive = true;
    bool canDoubleJump = false;

    // Cached component references
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeetCollider2D;

    //============================================================================================================
    // Functions
    //============================================================================================================
    //------------------------------------------------------------------------------------------------------------

    // get the player bounch sfx
//    public AudioClip GetBounceSFX() { return bounceSFX; }

    //------------------------------------------------------------------------------------------------------------

    // Use this for initialization
    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
    }

//------------------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;

        //BounceOffAnEnemy();
        Die();

        Run();
        Jump();
        UpdateAnimation();
    }

//------------------------------------------------------------------------------------------------------------

    // handle the player running
    private void Run()
    {
        if (!isAlive) return;

        // get the input axis
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");   // value is between -1 to 1
        
        // change the player velocity according to the axis value
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
    }

//------------------------------------------------------------------------------------------------------------

    // handle the player jumpping
    private void Jump()
    {
        if (!isAlive) return;

        // check if the jump buttun got pressed
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            bool isTouchingTheGround = myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));

            // if the player is on the ground do a normal jump
            if (isTouchingTheGround)
            {
                // change the player velocity according to the jump speed value
                DoAJump();

                // init the double jump flag
                canDoubleJump = true;
            }
            // else check if the player can double jump and do a double jump if he can
            else if(canDoubleJump)
            {
                DoubleJump();
            }
        }
    }

//------------------------------------------------------------------------------------------------------------

    // handle the player double jumping
    private void DoubleJump()
    {
        if (!isAlive) return;

        // if the player already double jumped once then he cant do it again, return false
        if (!canDoubleJump) return;

        // if the player is touching the ground he cant do a double jump, he should be up in the air
        bool isTouchingTheGround = myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if(!isTouchingTheGround)
        {
            // change the player velocity according to the jump speed value
            DoAJump();

            // trigger the animation of the double jump
            myAnimator.SetTrigger("doubleJumpTrigger");

            // change the bool doubleJumped to true, in order to limit the number of double jumps to 1
            canDoubleJump = false;
        }

    }

    //------------------------------------------------------------------------------------------------------------

    // makes the player jump by changing his velocity
    private void DoAJump()
    {
        if (!isAlive) return;

        // change the player velocity according to the jump speed value
        Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0f);
        myRigidbody.velocity += jumpVelocityToAdd;
    }

    //------------------------------------------------------------------------------------------------------------

    // Update the player animation according to his current state
    private void UpdateAnimation()
    {
        // handle the running animation
        HandleRunAnimation();

        // handle the jumping animation
        HandleJumpAnimation();

        // handle the falling animation
        HandleFallAnimation();

        // handle the flipping of the player sprite horizontally
        FlipSprite();
    }

//------------------------------------------------------------------------------------------------------------

    // handle the running animation
    private void HandleRunAnimation()
    {
        // check if the player is moving horizontally and change the isRunning bool of the animator accordingly
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        // check if the player is on the ground
        bool isTouchingTheGround = myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if(isTouchingTheGround) myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

//------------------------------------------------------------------------------------------------------------

    // handle the jumping animation
    private void HandleJumpAnimation()
    {      
        // if the player is touching the ground hes not jumping
        // also a jumping player must have a positive y axis velocity
        bool isTouchingTheGround = myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool isPlayerJumping = (!isTouchingTheGround && myRigidbody.velocity.y > 0);

        // if the player is jumping and didnt do a double jump change the animation to the normal jump animation
        if (isPlayerJumping)
        {
            myAnimator.SetBool("isJumping", true);
        }
        else
        {
            myAnimator.SetBool("isJumping", false);
        }
    }

//------------------------------------------------------------------------------------------------------------

    // handle the player falling animation
    private void HandleFallAnimation()
    {
        // if the player is touching the ground he cant fall
        bool isTouchingTheGround = myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (!isTouchingTheGround && myRigidbody.velocity.y < 0)
        {
            myAnimator.SetBool("isFalling", true);
        }
        else
        {
            myAnimator.SetBool("isFalling", false);
        }
    }

//------------------------------------------------------------------------------------------------------------

    // handle the flipping of the player sprite horizontally (left or right) according to the player x axis velocity
    private void FlipSprite()
    {
        // if the player is moving horizontally - reverse the current scaling of x axis
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), transform.localScale.y);
        }
    }

//------------------------------------------------------------------------------------------------------------

    // handle the player death
    private void Die()
    {
        bool isTouchingAnEnemy = myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy"));
        bool isBodyTouchingAProjectile = myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Projectile"));
        bool isFeetTouchingAProjectile = myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Projectile"));
        bool isBodyTouchingAnHazard = myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Hazards"));
        bool isFeetTouchingAnHazard = myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Hazards"));
        
        if (isTouchingAnEnemy || isBodyTouchingAProjectile || isFeetTouchingAProjectile || isBodyTouchingAnHazard || isFeetTouchingAnHazard)
        {
            isAlive = false;

            // make the rigid body static in order to make the player obj stay in place once the death animation plays
            myRigidbody.bodyType = RigidbodyType2D.Static;
            
            // trigger the death animation
            myAnimator.SetTrigger("Die");

            // wait for the death animation to play and then procces the player death
            Invoke("ProccesPlayerDeath", DeathAnimation.length);
        }
    }

//------------------------------------------------------------------------------------------------------------

    // tells the GameSession obj to handle the player death
    private void ProccesPlayerDeath()
    {
        FindObjectOfType<GameSession>().ProccesPlayerDeath();
    }


//------------------------------------------------------------------------------------------------------------

    /*  *** marked atm, the enemy weakspot got a material with bounce factor in it so theres no need for this function atm ***

    // make the player bounce off enemies
    private void BounceOffAnEnemy()
    {
        bool isTouchingAnEnemy = myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy"));

        if (isTouchingAnEnemy)
        {
            AudioSource.PlayClipAtPoint(bounceSFX, Camera.main.transform.position);
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, bounceOffEnemyFactor);
        }
    }
*/

//------------------------------------------------------------------------------------------------------------

}
