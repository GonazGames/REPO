using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class KinematicObject : MonoBehaviour
    {

        public float minGroundNormalY = .65f;

        public float gravityModifier = 1f;

        public Vector2 velocity;

        public bool IsGrounded { get; private set; }

        protected Vector2 targetVelocity;
        protected Vector2 groundNormal;
        protected Rigidbody2D body;
        protected ContactFilter2D contactFilter;
        protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
        private string run_ANIMATION = "run";
        private Animator anim;

        protected const float minMoveDistance = 0.001f;
        protected const float shellRadius = 0.01f;
		public int maxJumps = 2;

		protected int jumps;
		public float maxJumpVelocity = 10f;
        public float maxSpeed = 5f;

		protected float jumpSpeedModifier = 1f;

        /// <summary>
        /// Bounce the object's vertical velocity.
        /// </summary>
        /// <param name="value"></param>
        public void Bounce(float value)
        {
            velocity.y = value;
        }

        /// <summary>
        /// Bounce the objects velocity in a direction.
        /// </summary>
        /// <param name="dir"></param>
        public void Bounce(Vector2 dir)
        {
            velocity.y = dir.y;
            velocity.x = dir.x;
        }

        /// <summary>
        /// Teleport to some position.
        /// </summary>
        /// <param name="position"></param>
        public void Teleport(Vector3 position)
        {
            body.position = position;
            velocity *= 0;
            body.velocity *= 0;
        }

        protected virtual void OnEnable()
        {
            body = GetComponent<Rigidbody2D>();
            body.isKinematic = true;
        }

        protected virtual void OnDisable()
        {
            body.isKinematic = false;
        }

        protected virtual void Start()
        {
            contactFilter.useTriggers = false;
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            contactFilter.useLayerMask = true;
        }

        protected virtual void Update()
        {
            targetVelocity = Vector2.zero;
            ComputeVelocity();
			AnimatePlayer();
        }

        protected virtual void ComputeVelocity()
{
    // Only allow a jump if on the ground or if the number of jumps is less than the maximum.
    if ((IsGrounded || jumps < maxJumps) && Input.GetKeyDown(KeyCode.Space))
    {
        // Reset the vertical velocity before jumping.
        velocity.y = 0;

        // Limit the jump velocity.
        float jumpVelocity = maxJumpVelocity * jumpSpeedModifier;
        velocity += jumpVelocity * Vector2.up;
        jumps++;
    }
    // Check if the space key is being held down and the player is already jumping.
    else if (Input.GetKey(KeyCode.Space) && velocity.y > 0)
    {
        // Do not add to the jump velocity.
    }
    else if (IsGrounded)
    {
        jumps = 0;
    }

    targetVelocity = Vector2.zero;
    ComputeHorizontalVelocity();
}


		
		 protected virtual void ComputeHorizontalVelocity()
    {
        float xInput = Input.GetAxis("Horizontal");
        targetVelocity.x = xInput * maxSpeed;

        // Flip the sprite horizontally if moving left or right.
        if (xInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (xInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }


        protected virtual void FixedUpdate()
    {
    // Only apply gravity if not jumping.
    if (!Input.GetKey(KeyCode.Space))
    {
        // Fall faster than the jump speed.
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
    }

    // Apply horizontal velocity.
    velocity.x = targetVelocity.x;

    IsGrounded = false;

    var deltaPosition = velocity * Time.deltaTime;

    var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

    var move = moveAlongGround * deltaPosition.x;

    PerformMovement(move, false);

    move = Vector2.up * deltaPosition.y;

    PerformMovement(move, true);
    }


        void PerformMovement(Vector2 move, bool yMovement)
        {
            var distance = move.magnitude;

            if (distance > minMoveDistance)
            {
                //check if we hit anything in current direction of travel
                var count = body.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
                for (var i = 0; i < count; i++)
                {
                    var currentNormal = hitBuffer[i].normal;

                    //is this surface flat enough to land on?
                    if (currentNormal.y > minGroundNormalY)
                    {
                        IsGrounded = true;
                        // if moving up, change the groundNormal to new surface normal.
                        if (yMovement)
                        {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }
                    if (IsGrounded)
                    {
                        //how much of our velocity aligns with surface normal?
                        var projection = Vector2.Dot(velocity, currentNormal);
                        if (projection < 0)
                        {
                            //slower velocity if moving against the normal (up a hill).
                            velocity = velocity - projection * currentNormal;
                        }
                    }
                    else
                    {
                        //We are airborne, but hit something, so cancel vertical up and horizontal velocity.
                        velocity.x *= 0;
                        velocity.y = Mathf.Min(velocity.y, 0);
                    }
                    //remove shellDistance from actual move distance.
                    var modifiedDistance = hitBuffer[i].distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
            body.position = body.position + move.normalized * distance;
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
	

