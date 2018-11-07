using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    [SerializeField] private int MovementSpeed = 1;
    [SerializeField] public List<Transform> MovingPoints;

    public Transform CurrentMovePoint;
    int CurrentMovePointIndex;
    Rigidbody2D rb;

    int MoveDir = 1;

    // Use this for initialization
    void Awake () {

        rb = GetComponent<Rigidbody2D>();

        //Set the current point to the first in the index
        CurrentMovePointIndex = 0;
        CurrentMovePoint = MovingPoints[CurrentMovePointIndex];

	}
	
	// Update is called once per frame
	void Update () {

        //Get the direction the platform is going
        Vector2 dir = CurrentMovePoint.position - transform.position;

        //Check the distance to the move point
        if (Vector2.Distance(transform.position, CurrentMovePoint.position) < 1f) {

            //Check what direction the platform is moving
            if (dir.x > 0) {

                //Change the direction
                MoveDir = -1;

            }
            else if (dir.x < 0) {

                //Change the direction
                MoveDir = 1;

            }

            //Check if the platform isn't at the last point
            if (CurrentMovePointIndex < MovingPoints.Count - 1) {

                //Increase the index
                CurrentMovePointIndex++;

            }
            else {
                //Set the index to zero
                CurrentMovePointIndex = 0;
            }

            //Set the current moving point
            CurrentMovePoint = MovingPoints[CurrentMovePointIndex];

        }
        //Move the platform
        rb.velocity = new Vector3(MoveDir * MovementSpeed * Time.deltaTime, 0);
    }

    //Check if something is colliding with the platform
    private void OnCollisionEnter2D(Collision2D collision) {

        //If the object is the player
        if (collision.gameObject.GetComponent<PlayerMotor>()) {

            //Check all the points of contact in the collision
            foreach (ContactPoint2D hitPos in collision.contacts) {

                //If the contact point normal y is less than zero
                if (hitPos.normal.y < 0) {

                    //Set the parent to the platform
                    collision.gameObject.transform.parent = this.transform;

                }

            }

        }

    }

    //Called when the player jumps of the platform
    private void OnCollisionExit2D(Collision2D collision) {

        //Check if it's the player
        if (collision.gameObject.GetComponent<PlayerMotor>()) {

            //Set the players parent to null
            collision.gameObject.transform.parent = null;

        }

    }

}
