using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Move : MonoBehaviour
{
    // Public variables that can be set in the Inspector
    public float speed = 5f;            // Horizontal movement speed
    public float jumpForce = 10f;       // Jump force
    public float doubleJumpForce = 8f;  // Double jump force
    public float maxJumpVelocity = 10f; // Maximum upward velocity during jumps

    // Private variables that are not visible in the Inspector
    private bool isGrounded = true;     // Is the player on the ground?
    private bool canDoubleJump = false; // Can the player double jump?
    private string run_ANIMATION = "run";
    private Animator anim;

    private Rigidbody2D rb;            // Reference to the Rigidbody2D component

    void Start()
    {
        // Get the Rigidbody2D component on this GameObject
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 2f; // Set a higher gravity scale
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        AnimatePlayer();
        // Get the horizontal input axis (left/right arrow keys or A/D keys)
        float horizontal = Input.GetAxis("Horizontal");

        // Create a new Vector2 for the horizontal movement, with the desired speed and the current vertical velocity
        Vector2 movement = new Vector2(horizontal * speed, rb.velocity.y);

        // Limit the upward velocity during jumps
        if (movement.y > maxJumpVelocity)
        {
            movement.y = maxJumpVelocity;
        }

        // Set the Rigidbody2D velocity to the movement Vector2
        rb.velocity = movement;

        // Flip the sprite horizontally if moving left or right
        if (horizontal < 0)
        {
            transform.localScale = new Vector2(-1, 1);  // Flip left
        }
        else if (horizontal > 0)
        {
            transform.localScale = new Vector2(1, 1);   // Flip right
        }

        // Jumping code
        if (Input.GetKeyDown(KeyCode.Space))  // If the Space key is pressed
        {
            if (isGrounded)  // If the player is on the ground
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);  // Set the upward velocity to the jump force
                isGrounded = false;  // Set isGrounded to false so the player can't jump again until they land
                canDoubleJump = true; // Set canDoubleJump to true so the player can double jump
            }
            else if (canDoubleJump)  // If the player is in the air and can double jump
            {
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);  // Set the upward velocity to the double jump force
                canDoubleJump = false;  // Set canDoubleJump to false so the player can't double jump again until they land
            }
        }
    }

 void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Ground"))  // If the collision object has the "Ground" tag
    {
        isGrounded = true;     // Set isGrounded to true so the player can jump again
        canDoubleJump = false;  // Set canDoubleJump to false so the player can't double jump again until they jump from the ground

        // Check if the collision was with the side of the platform
        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contacts);

        foreach (ContactPoint2D contact in contacts)
        {
            if (Mathf.Abs(contact.normal.y) < 0.5f)  // If the collision normal is more horizontal than vertical
            {
                // Move the player to the side of the platform
                float direction = Mathf.Sign(contact.normal.x);
                transform.position += new Vector3(contact.normal.x * 0.1f, 0, 0);

                // Set the player's velocity to zero in the x direction
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }
}


    void AnimatePlayer()
{
    if (anim == null)
    {
        anim = GetComponent<Animator>();
    }

    float horizontal = Input.GetAxis("Horizontal");

    if (horizontal > 0)
    {
        anim.SetBool(run_ANIMATION, true);
        transform.localScale = new Vector2(1, 1);
    }
    else if (horizontal < 0)
    {
        anim.SetBool(run_ANIMATION, true);
        transform.localScale = new Vector2(-1, 1);
    }
    else
    {
        anim.SetBool(run_ANIMATION, false);
    }
}
}
