using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    public Rigidbody2D theRB;
    public float jumpForce;
    private bool isGrounded;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    private bool canDoubleJump;

    private Animator anim;
    private SpriteRenderer theSR;

    public float knockBackLenght, knockBackForce;
    private float knockBackCounter;

    public float bounceForce;

    public bool stopInput;

    private void Awake()
    {
        instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        theSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {//if they are both not true its false
        if (!PauseMenu.instance.isPaused && !stopInput)
        {

            if (knockBackCounter <= 0)
            {

                theRB.linearVelocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), theRB.linearVelocity.y);

                isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatIsGround);

                if (isGrounded)
                {
                    canDoubleJump = true;
                }

                if (Input.GetButtonDown("Jump"))
                {
                    if (isGrounded)
                    {
                        theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);

                        AudioManager.instance.PlaySFX(10);
                    }
                    else
                    {
                        if (canDoubleJump)
                        {
                            theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
                            canDoubleJump = false;

                            AudioManager.instance.PlaySFX(10);
                        }
                    }
                }

                if (theRB.linearVelocity.x < 0)
                {
                    theSR.flipX = true;
                }
                else if (theRB.linearVelocity.x > 0)
                {
                    theSR.flipX = false;
                }
            }
            else
            {
                knockBackCounter -= Time.deltaTime;
                //forcing-pushing player to the side he is facing (! not true)
                if (!theSR.flipX)
                {
                    theRB.linearVelocity = new Vector2(-knockBackForce, theRB.linearVelocity.y);
                }
                else
                {
                    theRB.linearVelocity = new Vector2(knockBackForce, theRB.linearVelocity.y);
                }
            }

        }

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("moveSpeed", Mathf.Abs(theRB.linearVelocity.x));
    }
    public void KnockBack()
    {
        knockBackCounter = knockBackLenght;
        theRB.linearVelocity = new Vector2(0f, knockBackForce);

        anim.SetTrigger("hurt");
    }

    public void Bounce()
    {
        theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, bounceForce);
        AudioManager.instance.PlaySFX(10);

        
    }
}
