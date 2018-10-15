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
        canJump = false;
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKey(KeyCode.A)) {
            rb.AddForce(Vector2.left * Acceleration * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D)) {
            rb.AddForce(Vector2.right * Acceleration * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W) && canJump) {
            rb.AddForce(Vector2.up * JumpForce * Time.deltaTime);
        }

    }
}
