using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    //public GameObject JumpSound;

    public float runSpeed = 2;

    public float jumpSpeed = 3;

    bool isTouchingFront = false;
    bool wallSliding;
    public float wallSlidingSpeed = 0.75f;
    bool isTouchingLeft;
    bool isTouchingRigth;

    Rigidbody2D rb2D;

    public bool betterJump = false;

    public float fallMultiplier = 0.5f;

    public float lowJumpMultiplier = 1f;

    //public Animator animator;

    public GameObject Counterattack;

    public SpriteRenderer spriteRenderer;
    void Start()
    {
        Counterattack.SetActive(false);
        rb2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey("right") && isTouchingRigth == false)
        {
            rb2D.velocity = new Vector2(runSpeed, rb2D.velocity.y);
            spriteRenderer.flipX = false;
            //animator.SetBool("Run", true);
        }
        else if (Input.GetKey("left") && isTouchingLeft == false)
        {
            rb2D.velocity = new Vector2(-runSpeed, rb2D.velocity.y);
            spriteRenderer.flipX = true;
            //animator.SetBool("Run", true);
        }
        else
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            //animator.SetBool("Run", false);
        }

        if (Input.GetKey("up") && CheckGround.isGrounded && wallSliding==false)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
            //Instantiate(JumpSound);
        }

        if (CheckGround.isGrounded == false)
        {
            //animator.SetBool("Jump", true);
            //animator.SetBool("Run", false);
        }
        if (CheckGround.isGrounded == true)
        {
            //animator.SetBool("Jump", false);
        }


        if (betterJump)
        {
            if (rb2D.velocity.y < 0)
            {
                rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;
            }

            if (rb2D.velocity.y > 0 && !Input.GetKey("space"))
            {
                rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier) * Time.deltaTime;
            }
        }

        if (isTouchingFront==true && CheckGround.isGrounded == false)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        if (wallSliding)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            Activate();
        }

        void Activate()
        {
            Counterattack.SetActive(true);
            Invoke("cooldown", 0.1f);
        }
    }

    void cooldown()
    {
        Counterattack.SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("WallRight"))
        {
            isTouchingFront = true;
            isTouchingRigth = true;
        }

        if (collision.gameObject.CompareTag("WallLeft"))
        {
            isTouchingFront = true;
            isTouchingLeft = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isTouchingFront = false;
        isTouchingLeft = false;
        isTouchingRigth = false;
    }
}