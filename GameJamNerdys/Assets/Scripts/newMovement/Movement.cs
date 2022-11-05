using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{

    private Collision coll;
    [HideInInspector]
    public Rigidbody2D rb;
    //private AnimationScript anim;

    [Space]
    [Header("Stats")]
    public float speed;
    public float jumpForce;
    public float slideSpeed;
    public float wallJumpLerp;
    public float dashSpeed;
    public Transform respawnPos;
    public GameObject Counterattack;

    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;
    public bool hasLife;
    [SerializeField]
    private bool gameRunning;


[Space]
    private bool groundTouch;
    //private bool hasDashed;

    public int side = 1;







    /*
    [Space]
    [Header("Polish")]
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;
    */

    private void Start()
    {
        Counterattack.SetActive(false);
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        // anim= GetComponentInChildren<AnimationScript>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxis("Horizontal");
        float yRaw = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(x, y);

        Walk(dir);
        // anim.SetHorizontalMovement(x,y,rb.velocity.y);

        if (coll.onWall && Input.GetButton("Fire3") && canMove)
        {
            if (side != coll.wallSide)
            {
                //anim.flip(side*-1);
            }
            wallGrab = true;
            wallSlide = false;
        }

        if (Input.GetButtonUp("Fire3") || !coll.onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        if (coll.onGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<BetterJumping>().enabled = true;
        }

        if (wallGrab && !isDashing)
        {
            rb.gravityScale = 0;
            if (x > .2f || x < -.2f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            float speedModifier = y > 0 ? .5f : 1;
            rb.velocity = new Vector2(rb.velocity.x, y * (speed * speedModifier));
        }
        else
        {
            rb.gravityScale = 3;
        }

        if (coll.onWall && !coll.onGround)
        {
            if (x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }

        if (!coll.onWall || coll.onGround)
        {
            wallSlide = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            //anim.setTrigger("jump");

            if (coll.onGround)
                Jump(Vector2.up, false);
            if (coll.onWall && !coll.onGround)
                WallJump();
        }


        if(coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if(!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        //WallParticle(y);

        if (wallGrab || wallSlide || !canMove)
            return;

        if (x > 0)
        {
            side = 1;
            //anim.Flip(side);
        }
        if (x < 0)
        {
            side = -1;
            //anim.Flip(side);
        }

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

    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.CompareTag("changeJump") && (Input.GetKey("e")))
    //    {
    //        Time.timeScale = 0.1f;

    //    }
    //}

    //public void ChangeGameRunningState()
    //{
    //    gameRunning = !gameRunning;

    //    if (gameRunning)
    //    {
    //        Time.timeScale = 1f;
    //    }
    //    else
    //    {
    //        Time.timeScale = 0.1f;
    //    }
    //}

    void cooldown()
    {
        Counterattack.SetActive(false);
    }
    private void Walk(Vector2 dir) 
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        } else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp*Time.deltaTime);
        }
    }

    private void WallSlide()
    {
        if (coll.wallSide != side)
        {
            //anim.flip(side*-1)

        }

        if (!canMove)
            return;

        bool pushingWall = false;
        if ((rb.velocity.x > 0 && coll.onRightWall) || (rb.velocity.x < 0 && coll.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;
        rb.velocity = new Vector2(push, -slideSpeed);
    }

    private void Jump(Vector2 dir, bool wall)
    {
        //slideParticle.transform.parent.localScale=new Vector3(ParticleSide(),1,1);
        //ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;

        // particle.Play();
    }

    private void WallJump()
    {
        if((side==1 && coll.onRightWall) || side==-1 && !coll.onRightWall)
        {
            side *= -1;
            //anim.Flip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;
        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);
        wallJumped = true;
    }

    private void Dash(float x, float y)
    {
        //Camera.main.transform.DOCompplete


        //hasDashed = true;
        //anim.setTrigger("dash");

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());

    }

    IEnumerator DashWait()
    {
        //FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());

        //dashParticle.Play();
        rb.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(0.3f);

        //dashParticle.Stop();
        rb.gravityScale = 3;
        GetComponent<BetterJumping>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(0.15f);
        //if (coll.onGround)
            //hasDashed = false;
    }

    IEnumerator DisableMovement (float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    private void GroundTouch()
    {
        //hasDashed = false;
        isDashing = false;

        // side= anim.sr.flipX ? -1 : 1;
        // jumpParticle.Play();
    }

    /*
    void WallParticle(float vertical)
    {
        var main = slideParticle.main;

        if(wallSlide || (wallGrab && vertical < 0))
        {
            slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
            main.startColor = Color.white;
        }else
        {
            main.startColor = Color.clear;
        }
    }

    int ParticleSide()
    {
        int particleSide = coll.onRigthWall ? 1 : -1;
        return particleSide;
    }
    */





    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            transform.position = respawnPos.position;
        }
    }


}
