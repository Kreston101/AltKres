using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStates playerState;
    private Rigidbody2D rb2d;
    [SerializeField] private float speed = 5;
    private float horizontal;
    private float vertical;

    [SerializeField] private float jumpForce = 10;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] public Vector3 lastGroundedPos;

    private float jumpBufferCount = 0;
    [SerializeField] private float jumpBufferFrames;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime;
    private int extraJumps = 0;
    [SerializeField] private int maxExtraJumps;

    private bool canDash = true;
    private bool dashed = false;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    private bool attacking;
    [SerializeField] private float attackCooldown;
    private float attackCDTimer;
    [SerializeField] private int damage;
    [SerializeField] private Transform front, above, below;
    [SerializeField] private Vector3 FrontArea, AboveArea, BelowArea;
    [SerializeField] private LayerMask attackableLayer;

    [SerializeField] private int bulletCount = 3;
    [SerializeField] private int maxBullets = 9;
    [SerializeField] private GameObject bulletFab;

    public int health = 9;
    public int maxHealth = 9;

    private float gravity;

    public static PlayerController Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
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
        if (!playerState.dashing && !playerState.damaged)
        {
            Move();
            Jump();
        }
        MeleeAtk();
        RangedAttack();
    }

    private void GetInputs()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        attacking = Input.GetKeyDown(KeyCode.X);
    }

    private void Move()
    {
        rb2d.velocity = new Vector2(horizontal * speed, rb2d.velocity.y);
    }

    private void StartDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !dashed && playerState.unlockedDash)
        {
            StartCoroutine(Dash());
            dashed = true;
            GetLastGroundedPosition();
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
            || Physics2D.Raycast(groundCheck.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, groundLayer)
            || Physics2D.Raycast(groundCheck.position - new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, groundLayer))
        {
            if (Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckY, groundLayer)
            && Physics2D.Raycast(groundCheck.position + new Vector3(1, 0, 0), Vector2.down, groundCheckY, groundLayer)
            && Physics2D.Raycast(groundCheck.position - new Vector3(1, 0, 0), Vector2.down, groundCheckY, groundLayer))
            {
                GetLastGroundedPosition();
            }
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
            else if (!isGrounded() && extraJumps < maxExtraJumps && Input.GetButtonDown("Jump") && playerState.unlockedExtraJump)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                playerState.jumping = true;
                extraJumps++;
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
        if (horizontal > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
        else if (horizontal < 0)
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
            extraJumps = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            GetLastGroundedPosition();
            jumpBufferCount = jumpBufferFrames;
        }
        else
        {
            jumpBufferCount--;
        }
    }

    private void MeleeAtk()
    {
        attackCDTimer += Time.deltaTime;
        if (attacking && attackCDTimer >= attackCooldown)
        {
            attackCDTimer = 0;
            if (vertical == 0 || vertical < 0 && isGrounded())
            {
                Hit(front, FrontArea);
            }
            else if (vertical > 0)
            {
                Hit(above, AboveArea);
            }
            else if (vertical < 0 && !isGrounded())
            {
                Hit(below, BelowArea);
            }
        }
    }

    private void Hit(Transform attackTransform, Vector2 attackArea)
    {
        Collider2D[] objectsHit = Physics2D.OverlapBoxAll(attackTransform.position, attackArea, 0, attackableLayer);

        foreach(Collider2D objectHit in objectsHit)
        {
            if (objectHit.CompareTag("Enemy"))
            {
                Debug.Log(objectHit.name);
                if (objectHit.GetComponent<Enemy>())
                {
                    objectHit.GetComponent<Enemy>().StartCoroutine("TakeDamage", damage);
                }
                else if (objectHit.GetComponent<Flyer>())
                {
                    objectHit.GetComponent<Flyer>().StartCoroutine("TakeDamage", damage);
                }
            }
            else if (objectHit.CompareTag("Breakable"))
            {
                Debug.Log(objectHit.name);
                objectHit.GetComponent<BreakableObject>().TakeDamage(damage);
            }
        }
    }

    private void RangedAttack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && bulletCount > 0)
        {
            GameObject holder = Instantiate(bulletFab, transform.position, Quaternion.identity);
            holder.GetComponent<Bullet>().direction = transform.localScale.x;
            holder.GetComponent<Bullet>().damage = damage * 3;
            bulletCount--;
        }
    }

    public IEnumerator TakeDamage(int damage, float direction)
    {
        //tookDamage = true;
        playerState.damaged = true;
        rb2d.gravityScale = 0;
        rb2d.velocity = new Vector2(direction * 15, 0);
        yield return new WaitForSeconds(0.25f);
        rb2d.gravityScale = gravity;
        playerState.damaged = false;
        //tookDamage = false;
    }

    private void GetLastGroundedPosition()
    {
        lastGroundedPos = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(front.position, FrontArea);
        Gizmos.DrawWireCube(above.position, AboveArea);
        Gizmos.DrawWireCube(below.position, BelowArea);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            transform.position = lastGroundedPos;
            Debug.Log(lastGroundedPos);
        }
    }
}
