using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDataPersistence
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

    public int health = 3;
    public int maxHealth = 9;

    private float gravity;

    [SerializeField] private Animator anim;

    public static PlayerController Instance { get; set; }

    //RESPAWN SYSTEM
    //Most recent save point or start of game
    //Do not reload the blank save file
    //Figure out penalties
    //Scene of save point, coordinates, and how to load them
    //How to fit those ito save system (should be under the same spot where you load the game)
    //Should be in he progress persistence obj
    //shouldnt be that bad..?
    //make the load into scene first

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("new player instance");
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
        if(health <= 0)
        {
            gameObject.SetActive(false);
            FindObjectOfType<GameManager>().RespawnPlayer();
        }
    }

    private void GetInputs()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        attacking = Input.GetKeyDown(KeyCode.X);
        if (horizontal != 0)
        {
            anim.SetBool("Movement", true);
        }
        else anim.SetBool("Movement", false);
    }

    private void Move()
    {
        rb2d.velocity = new Vector2(horizontal * speed, rb2d.velocity.y);
    }

    private void StartDash()
    {
        if (playerState.unlockedDash)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !dashed)
            {
                StartCoroutine(Dash());
                dashed = true;
                GetLastGroundedPosition();
            }
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
                anim.SetBool("Grounded", false);
            }
            else if (!isGrounded() && extraJumps < maxExtraJumps && Input.GetButtonDown("Jump") && playerState.unlockedExtraJump)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                playerState.jumping = true;
                extraJumps++;
                anim.SetBool("Grounded", false);
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
            anim.SetBool("Grounded", true);
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            anim.SetBool("Grounded", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            GetLastGroundedPosition();
            jumpBufferCount = jumpBufferFrames;
            anim.SetBool("Grounded", false);
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
            anim.SetTrigger("Attack");
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
                else if (objectHit.GetComponent<InkBossController>())
                {
                    objectHit.GetComponent<InkBossController>().TakeDamage(damage);
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

    public IEnumerator TakeDamage(int damage, float direction, Collider2D other, float knockBack) //15? for normal 25 for boss
    {
        if (!playerState.damaged)
        {
            playerState.damaged = true;
            health -= damage;
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), other, true);
            rb2d.gravityScale = 0;
            rb2d.velocity = new Vector2(direction * knockBack, 0);
            yield return new WaitForSeconds(0.5f);
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), other, false);
            rb2d.gravityScale = gravity;
            playerState.damaged = false;
        }
        else
        {
            yield return null;
        }
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

    public void LoadData()
    {
        maxHealth = Progress.Instance.progPlayerMaxHealth;
        health = Progress.Instance.progPlayerCurrentHealth;
    }

    public void SaveData()
    {
        Progress.Instance.progPlayerCurrentHealth = health;
        Progress.Instance.progPlayerMaxHealth = maxHealth;
    }
}
