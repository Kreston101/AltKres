using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField] private float speed = 5;
    private float horizontal;

    [SerializeField] private float jumpForce = 10;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask groundLayer;

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
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInputs();
        Move();
        Jump();
    }

    private void FixedUpdate()
    {
        
    }

    private void GetInputs()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        rb2d.velocity = new Vector2(horizontal * speed, rb2d.velocity.y);
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
        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }
        if (Input.GetButtonUp("Jump") && rb2d.velocity.y > 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
    }
}
