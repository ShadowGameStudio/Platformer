using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class EndDoor : MonoBehaviour {

    [SerializeField] private bool IsLast = false;
    private int Level;


	// Use this for initialization
	void Awake () {

        //Gets the scenes current index
        Level = SceneManager.GetActiveScene().buildIndex;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision) {

        //Shows that the player has entered
        TransferToNextLevel();


    }

    void TransferToNextLevel() {

        //Add one to the index and load that scene
        //If it's not already in the last level
        if (!IsLast) {

            Level++;
            SceneManager.LoadScene(Level);
        }
        else {

            //Show end screen
        }


    }

}
