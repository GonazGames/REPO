using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Move : MonoBehaviour
{
    // Public variables that can be set in the Inspector
    public float speed = 5f;            // Horizontal movement speed
    public float jumpForce = 10f;       // Jump force
    public float doubleJumpForce = 8f;  // Double jump force

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
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        AnimatePlayer();
        // Get the horizontal input axis (left/right arrow keys or A/D keys)
        float horizontal = Input.GetAxis("Horizontal");

        // Create a new Vector2 for the horizontal movement, with the desired speed and the current vertical velocity
        Vector2 movement = new Vector2(horizontal * speed, rb.velocity.y);

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
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);  // Add an upward force to the Rigidbody2D
                isGrounded = false;  // Set isGrounded to false so the player can't jump again until they land
                canDoubleJump = true; // Set canDoubleJump to true so the player can double jump
            }
            else if (canDoubleJump)  // If the player is in the air and can double jump
            {
                rb.AddForce(new Vector2(0, doubleJumpForce), ForceMode2D.Impulse);  // Add an upward force to the Rigidbody2D
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
