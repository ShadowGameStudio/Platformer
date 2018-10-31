using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    public PlayerMotor motor;
    public float PlayerSpeed = 0f;

    private float HorizontalMove = 0f;
    private bool ShouldJump = false;
	
	// Update is called once per frame
	void Update () {

        HorizontalMove = Input.GetAxisRaw("Horizontal") * PlayerSpeed;

        if (Input.GetButtonDown("Jump")) {

            ShouldJump = true;

        }
        else {
            ShouldJump = false;
        }

        //Get the left mouse button
        if (Input.GetMouseButtonDown(0)) {

            //Make the player shoot
            Shoot();

        }

	}

    private void FixedUpdate() {

        //Move the player
        motor.Move(HorizontalMove, ShouldJump);

    }

    void Shoot() {

    }
}
