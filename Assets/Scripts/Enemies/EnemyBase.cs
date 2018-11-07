using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase : MonoBehaviour {

    [Header("Basic Enemy")]
    [SerializeField] private int EnemyHealth;
    [SerializeField] private LayerMask WhatIsGround;

    [SerializeField] private float GroundedRadius = 1f;

    [Header("Patrolling")]
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private int PlayerDetectionRange;

    [Header("Movement")]
    [SerializeField] private int JumpForce;
    [Range(0, 0.3f)] [SerializeField] private float MovementSmoothing = 0.05f;
    [SerializeField] private float EnemySpeed;

    [SerializeField] private float FallMultiplier = 2.5f;
    [SerializeField] private float ExtraGravity = 0f;
    [SerializeField] private float WallDetectionRange = 1f;

    public Transform CurrentPatrolPoint;
    private GameObject GroundCheck;
    public List<Transform> PatrolPoints;

    int CurrentPatrolPointIndex;
    private Rigidbody2D rb;
    private Vector3 Velocity = Vector3.zero;

    private Vector2 Dir = Vector2.zero;
    private bool FacingRight = false;
    private bool IsChasingPlayer = false;

    private bool Grounded = true;

	// Use this for initialization
	void Awake () {

        GroundCheck = GameObject.FindGameObjectWithTag("WorldGenerator");

        //Get the enemy's rigidbody
        rb = GetComponent<Rigidbody2D>();

        //Set the start patrol point
        CurrentPatrolPointIndex = 0;
        CurrentPatrolPoint = PatrolPoints[CurrentPatrolPointIndex];

	}

    // Update is called once per frame
    void Update () {

        //Get the direction the enemy is currently facing
        Dir = CurrentPatrolPoint.position - transform.position;

        //Check if the distance to the patrol point is less than 1f
        if (Vector2.Distance(transform.position, CurrentPatrolPoint.position) < 1f)
        {

            //Check what direction the enemy is travelling and change it
            //to the opposite
            if (Dir == Vector2.right)
            {

                Dir = Vector2.left;

            }
            else if (Dir == Vector2.left)
            {

                Dir = Vector2.right;

            }

            //Check if we haven't reached the end of the array
            if (CurrentPatrolPointIndex < PatrolPoints.Count - 1)
            {

                //Increase the index
                CurrentPatrolPointIndex++;

            }
            else
            {

                //Set the index to zero
                CurrentPatrolPointIndex = 0;

            }
            //Set the current patrol point
            CurrentPatrolPoint = PatrolPoints[CurrentPatrolPointIndex];

        }

        //Make the enemy move
        Move(Dir.x);
    }

    //FixedUpdate is called every physics frame
    private void FixedUpdate() {

        //Check if the player is near
        CheckForPlayer();
        CheckIfGrounded();

        Vector2 forward = Vector2.zero;
        //If the facing direction is right
        if (Dir.x > 0) {

            //Set the ray direction to right
            forward = transform.TransformDirection(Vector2.right);

        }
        //If the facing direction is left
        else if (Dir.x < 0) {

            //Set the ray direction to left
            forward = transform.TransformDirection(Vector2.left);

        }

        int layerMask = 1 << 9;

        //Check if there is an object in front of the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, forward, WallDetectionRange, layerMask);
        if (hit.collider != null) {

            //Make the enemy jump
            Jump();

        }

        //If the enemy is falling
        if (rb.velocity.y < 0) {

            //Make the enemy fall faster
            rb.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.fixedDeltaTime;

        }

        //If the enemy is jumping
        if (rb.velocity.y > 0) {

            //Get the velocity and change it
            Vector2 vel = rb.velocity;
            vel.y -= ExtraGravity * Time.fixedDeltaTime;
            rb.velocity = vel;

        }

    }

    //Moves the enemy
    void Move(float move) {

        //Set the target velocity
        Vector3 targetVelocity = new Vector2(move * EnemySpeed, rb.velocity.y);

        //Move the player
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref Velocity, MovementSmoothing);

    }

    //Makes the enemy jump
    void Jump() {

        //If the enemy is chasing the player and is grounded
        if (IsChasingPlayer && Grounded) {

            //Add force upwards to make the enemy jump
            rb.AddForce(new Vector2(0, JumpForce));

        }

    }

    //Check if the enemy is on the ground
    void CheckIfGrounded() {

        //Get all the nearby colliders within the ground layer
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(GroundCheck.transform.position, GroundedRadius, WhatIsGround);

        //Go through all the colliders
        for (int i = 0; i < hitColliders.Length; i++) {

            //It the collider isn't this
            if (hitColliders[i].gameObject != gameObject) {

                //Set grounded to true
                Grounded = true;
            }

        }

    }

    //Checks if the player is nearby
    void CheckForPlayer() {

        //Get all the nearby colliders
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, PlayerDetectionRange);

        //Go through all the elements in hitColliders
        for (int i = 0; i < hitColliders.Length; i++) {

            //Check if it is the player
            if (hitColliders[i].GetComponent<PlayerMotor>()) {

                //Check if the enemy is less than 10f from the player
                if (Vector2.Distance(transform.position, hitColliders[i].transform.position) < 10f) {

                    //Set the point to move towards to the player
                    CurrentPatrolPoint = hitColliders[i].transform;
                    IsChasingPlayer = true;

                }
                else {

                    //If the player is not "visible" go back to the normal points
                    CurrentPatrolPoint = PatrolPoints[CurrentPatrolPointIndex];
                    IsChasingPlayer = false;

                }

            }

        }

    }

    //Flips the enemy
    void Flip() {

        //Set facing direction and rotate the enemy
        FacingRight = !FacingRight;

        transform.Rotate(0f, 180f, 0f);

    }

}
