using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour {

    [SerializeField] private float JumpForce = 400f;
    [SerializeField] private float GroundedRadius = 1f;
    [SerializeField] private bool AirControl = false;

    [Range(0, 0.3f)] [SerializeField] private float MovementSmoothing = 0.05f;
    [SerializeField] private LayerMask WhatIsGround;

    [SerializeField] private float FallMultiplier = 2.5f;
    [SerializeField] private float ExtraGravity = 0f;

    private Rigidbody2D rb;
    private Vector3 Velocity = Vector3.zero;
    private GameObject GroundCheck;

    private bool Grounded = true;
    private bool FacingRight = false;


    // Use this for initialization
    void Awake() {

        GroundCheck = GameObject.FindGameObjectWithTag("WorldGenerator");

        if (GroundCheck == null)
        {
            Debug.Log("GROUND CHECK IS NULL");
        }

        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate() {

        //Check if player is touching ground
        //Get all the colliders nearby
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(GroundCheck.transform.position, GroundedRadius, WhatIsGround);

        //Go through all the colliders
        for (int i = 0; i < hitColliders.Length; i++) {

            //If the collider is this
            if (hitColliders[i].gameObject != gameObject) {
                
                //Set grounded to true
                Grounded = true;

            }

        }

        //Check if the player is falling
        if (rb.velocity.y < 0) {

            //Change the gravity to make the fall feel more "snappy"
            rb.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.fixedDeltaTime;

        }

        //If the player is jumping
        if (rb.velocity.y > 0) {

            Vector2 vel = rb.velocity;
            vel.y -= ExtraGravity * Time.fixedDeltaTime;
            rb.velocity = vel;

        }

    }

    //Moves the player
    public void Move(float move, bool jump) {

        //If the player is grounded or has air control enabled
        if (Grounded || AirControl) {

            //Set the target velocity of the player
            Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);

            //Move the player
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref Velocity, MovementSmoothing);

        }

        //Check if the player should jump
        if (jump && Grounded) {

            //Make the player jump
            Grounded = false;
            rb.AddForce(new Vector2(0, JumpForce));

        }

        //If the move variable is less than zero
        if (move < 0 && FacingRight) {

            Flip();

        }
        //If the move variable is greater than zero
        else if (move > 0 && !FacingRight) {

            Flip();

        }

    }

    //Flips the player
    private void Flip() {

        FacingRight = !FacingRight;

        transform.Rotate(0f, 180f, 0f);
    }

}
