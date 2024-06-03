using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool jump = false;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 100f;
    public Transform groundCheck;

    private bool grounded = false;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal"); //get the current input of h

        if (h * rb2d.velocity.x < maxSpeed) //if the character is already at max speed
        {
            rb2d.AddForce(Vector2.right * h * moveForce); //add horizontal force to the player
        }

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed) //if player not at the max speed
        {
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y); //add horizontal force to player
        }

        if (jump)
        {
            rb2d.AddForce(new Vector2(0f, jumpForce)); //adds force on the y axis
            jump = false; //sets jump to false to prevent double jumps
        }
    }
}
