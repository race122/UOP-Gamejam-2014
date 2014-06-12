/*
    File:           Rock.cs
    Author:         Krz
    Project:        Curling Game
    Soundtrack:     Station 90 Show 13: Simon Heartfield and Manni Dee
    Description:    The puck-like objects the players hit around
*/

using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour
{
    // --------------------------------------
    // game objects
    // --------------------------------------
    public Camera rockCamera;
    private Player player;
	public AudioClip rockCollision;

    // --------------------------------------
    // local variables
    // --------------------------------------
    public GameManager.eTeam team;
    private bool inSupply, isPickedUp, isFiring;
    private float frictionValue;

    private float FRICTION_MAX =                0.4f;
    private float FRICTION_MIN =                0.2f;

    private Vector3 BULLSEYE_POSITION;
    private Vector3 BACK_OF_HOUSE_POSITION;
    private Vector3 LAST_HACK_POSITION;

    // --------------------------------------
    // functions
    // --------------------------------------
    void Start() {
        player =                    FindObjectOfType<Player>();
        inSupply =                  true;
        isPickedUp =                false;
        isFiring =                  false;
        frictionValue =             FRICTION_MAX;
        BULLSEYE_POSITION =         GameObject.FindGameObjectWithTag("Bullseye").transform.position;
        BACK_OF_HOUSE_POSITION =    GameObject.FindGameObjectWithTag("BackOfHouse").transform.position;
        LAST_HACK_POSITION =        GameObject.FindGameObjectWithTag("LastHack").transform.position;
        // NEED TO ADD: GameObjects with the tags above at the correct locations
    }

    void Update() {
        StopMovingSlow();
        UpdateFriction();
        UpdateCamera();
    }

    public float DistanceFromBullseye() {
        return (transform.position - BULLSEYE_POSITION).magnitude;
    }

    public bool IsBeyondFinalHack() {
        return ((transform.position.z - BACK_OF_HOUSE_POSITION.z) > 0);
    }

    public void Pickup() {
        inSupply =              false;
        isPickedUp =            true;
    }

    public void Fire() {
        isPickedUp =            false;
        isFiring =              true;
    }

    public bool InSupply() {
        return inSupply;
    }

    public bool IsPickedUp() {
        return isPickedUp;
    }

    public bool IsMoving() {
        return (rigidbody.velocity.magnitude > 0.05f);
    }

    public void StopMovingSlow() {
        if (rigidbody.velocity.magnitude > 0 && !IsMoving()) {
            rigidbody.velocity = Vector3.zero;
        }
    }

    private void UpdateFriction() {
        if (IsMoving()) {
            rigidbody.AddForce(rigidbody.velocity * frictionValue * -1f);
        }
    }

    // Send a value 0-1 to this function to set the friction value 
    // (0 = lowest, 1 = highest)
    public void SetFriction(float friction) {
        frictionValue = friction * FRICTION_MAX;
        
        if (frictionValue < FRICTION_MIN) {
            frictionValue = FRICTION_MIN;
        } else if (frictionValue > FRICTION_MAX) {
            frictionValue = FRICTION_MAX;
        }
    }

    private void UpdateCamera() {
        if (isFiring) {
            print(rigidbody.velocity.magnitude);
            if (!IsMoving()) {
                player.StoneFired();
                isFiring = false;
            }
        }
    }

	private void OnCollisionEnter( Collision col )
	{
		if ( ( col.transform.name == "Stone Blue" ) || ( col.transform.name == "Stone Red" ) )
		{
			AudioSource.PlayClipAtPoint ( rockCollision ,transform.position );
		}
	}
}