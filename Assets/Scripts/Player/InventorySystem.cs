using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour {

    private List<GameObject> Inventory = new List<GameObject>();

    [SerializeField] private int MaxInventorySize = 1;
    [SerializeField] private float MaxPickupDistance = 0f;
    [SerializeField] private Text MessageText;

    [SerializeField] private Weapon[] Weapons;

    private GameObject ObjectToPickup;
    private GameObject PlayerHand;

    private bool IsReadyToPickup = false;

    //Adds an item to the inventory
    void PickupItem(GameObject itemToAdd) {

        //Add the object to the inventory list
        if (AddItemToInventory(itemToAdd)) {

            //Get the player hand and set the object position
            PlayerHand = this.gameObject.transform.Find("Hand").gameObject;
            itemToAdd.transform.parent = PlayerHand.transform;

            //Remove the rigidbody from the object
            Rigidbody2D rb;

            //Get the rigidbody
            if (rb = itemToAdd.GetComponent<Rigidbody2D>()) {

                //Destroy it
                Destroy(rb);

                //Resets the object transform and rotation
                itemToAdd.transform.localPosition = new Vector3(0f, 0f, 0f);
                itemToAdd.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

            }
        } 
    }

    //Drops an item
    void DropItem() {

    }

    //Gets an item
    void GetItem() {

    }

    //Adds the item to the inventory
    bool AddItemToInventory(GameObject objectToAdd) {

        //Add the object to the players inventory
        Inventory.Add(objectToAdd);

        //Get the rigid body and set it's mass to the items mass
        Rigidbody2D rb;
        Item item;

        //Check if it can get the objects components
        if (rb = objectToAdd.GetComponent<Rigidbody2D>()) {

            //Get the item component
            if (item = objectToAdd.GetComponent<Item>()) {

                //Set the mass
                item.SetObjectMass(rb.mass);
                return true;
            }

        }
        else {
            return false;
        }

        return false;

    }
	
	// Update is called once per frame
	void Update () {

        //Get all the colliding objects near the player
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, MaxPickupDistance);

        //Go through all the colliders
        for (int i = 0; i < hitColliders.Length; i++) {

            //Check if the object has the item component
            if (hitColliders[i].GetComponent<Item>()) {

                //Check if the inventory size is less than max
                if (Inventory.Count < MaxInventorySize) {

                    //Activate message to pick it up
                    MessageText.text = "Press F to pickup";
                    MessageText.enabled = true;
                    IsReadyToPickup = true;

                    //Set the object to pickup to this object
                    ObjectToPickup = hitColliders[i].gameObject;
                    break;
                }
                else {

                    for (int y = 0; y < MaxInventorySize; y++) {

                        if (hitColliders[i] != Inventory[y]) {

                            //Activate the message to pick it up
                            MessageText.text = "Press F to pickup";
                            MessageText.enabled = true;
                            IsReadyToPickup = true;

                            ObjectToPickup = hitColliders[i].gameObject;
                            break;

                        }
                        break;
                    }

                }

            }
            else {

                //Disable the text
                MessageText.enabled = false;
                IsReadyToPickup = false;

                ObjectToPickup = null;
            }

        }

        //If the player can pick stuff up
        if (IsReadyToPickup) {

            //If the player presses F
            if (Input.GetKeyDown(KeyCode.F)) {

                //check the inventory size
                if (Inventory.Count == MaxInventorySize) {

                    //Drop an item in the inventory

                }
                else {
                    PickupItem(ObjectToPickup);
                }

            }

        }

	}
}
