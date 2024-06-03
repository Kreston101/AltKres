using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 2.0f;
    public float jumpForce = 1.0f;
    private Rigidbody2D rb2d;
    private Vector3 playerVelocity;
    private bool onGround = false;
    
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }
}