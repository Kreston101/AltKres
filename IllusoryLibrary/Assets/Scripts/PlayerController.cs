using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerStates playerState;
    private Rigidbody2D rb2d;
    [SerializeField] private float speed = 5;
    private float horizontal;

    [SerializeField] private float jumpForce = 10;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    private float jumpBufferCount = 0;
    [SerializeField] private float jumpBufferFrames;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime;

    private bool canDash = true;
    private bool dashed = false;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    [SerializeField] private float damage;
    [SerializeField] private Transform front, above, below;
    [SerializeField] private Vector3 FrontArea, AboveArea, BelowArea;

    private float gravity;

    public static PlayerController Instance { get; set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        playerState = GetComponent<PlayerStates>();
        rb2d = GetComponent<Rigidbody2D>();
        gravity = rb2d.gravityScale;
    }

    private void Update()
    {
        GetInputs();
        UpdateJump();
        Flip();
        StartDash();
        if(!playerState.dashing)
        {
            Move();
            Jump();
        }
        MeleeAtk();
    }

    private void GetInputs()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        rb2d.velocity = new Vector2(horizontal * speed, rb2d.velocity.y);
    }

    private void StartDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !dashed)
        {
            StartCoroutine(Dash());
            dashed = true;
        }
        if (isGrounded())
        {
            dashed = false;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        playerState.dashing = true;
        rb2d.gravityScale = 0;
        rb2d.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        yield return new WaitForSeconds(dashTime);
        rb2d.gravityScale = gravity;
        playerState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public bool isGrounded()
    {
        if (Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckY, groundLayer)
            || Physics2D.Raycast(groundCheck.position + new Vector3(groundCheckX,0,0), Vector2.down, groundCheckY, groundLayer)
            || Physics2D.Raycast(groundCheck.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Jump()
    {
        if (!playerState.jumping)
        {
            if (jumpBufferCount > 0 && coyoteTimeCounter > 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                playerState.jumping = true;
            }
        }
        if (Input.GetButtonUp("Jump") && rb2d.velocity.y > 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            playerState.jumping = false;
        }
    }

    private void Flip()
    {
        if(horizontal > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
        else if(horizontal < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
    }

    void UpdateJump()
    {
        if (isGrounded())
        {
            playerState.jumping = false;
            coyoteTimeCounter = coyoteTime; 
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCount = jumpBufferFrames;
        }
        else
        {
            jumpBufferCount--;
        }
    }

    private void MeleeAtk()
    {
        Collider2D[] collisions;
        if (Input.GetKeyDown(KeyCode.X) && Input.GetAxis("Vertical") == 0)
        {
            collisions = Physics2D.OverlapBoxAll(front.position, FrontArea, 0);
        }
        else if (Input.GetKeyDown(KeyCode.X) && Input.GetAxis("Vertical") > 0)
        {
            collisions = Physics2D.OverlapBoxAll(above.position, AboveArea, 0);
        }
        else if (Input.GetKeyDown(KeyCode.X) && Input.GetAxis("Vertical") < 0 && isGrounded())
        {
            collisions = Physics2D.OverlapBoxAll(front.position, FrontArea, 0);
        }
        else if (Input.GetKeyDown(KeyCode.X) && Input.GetAxis("Vertical") < 0 && !isGrounded())
        {
            collisions = Physics2D.OverlapBoxAll(below.position, BelowArea, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(front.position, FrontArea);
        Gizmos.DrawWireCube(above.position, AboveArea);
        Gizmos.DrawWireCube(below.position, BelowArea);
    }
}
