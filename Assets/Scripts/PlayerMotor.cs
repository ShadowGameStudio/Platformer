using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour {

    [SerializeField] float Acceleration;
    [SerializeField] float DeAcceleration;
    [SerializeField] float JumpForce;

    Rigidbody2D rb;
    bool canJump = true;

    // Use this for initialization
    void Start() {

        rb = GetComponent<Rigidbody2D>();

    }

    void OnCollisionEnter2D() {
        canJump = true;
    }

    void OnCollisionExit2D() {
    }

    // Update is called once per frame
    void Update() {

        //if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && canJump) {
        //    Vector2 vel = rb.velocity;
        //    vel.x /= DeAcceleration;

        //    rb.velocity = vel;
        //}

        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector2 _movHorizontal = transform.right * _xMov;
        Vector2 _movVerical = transform.up * _zMov;

        //Check if the vertical movement isn't equal to zero
        if (_movVerical != Vector2.zero) {

            canJump = false;
        }

        Vector2 _velocity = Vector2.zero;

        if (canJump) {
            //Final movement vector
            _velocity = (_movHorizontal + _movVerical).normalized * Acceleration;
        }
        else if (!canJump) {

            //Final movement vector
            _velocity = _movHorizontal.normalized * Acceleration;

        }

        rb.AddRelativeForce(_velocity);

    }
}
