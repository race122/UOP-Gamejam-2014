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
	public AudioClip[] commentatorSounds;
    public SphereCollider sphereCollider;

    // --------------------------------------
    // local variables
    // --------------------------------------
    public GameManager.eTeam team;
    private bool inSupply, isPickedUp, isFiring, hasJustFired;
    private float frictionValue;
    private float slowestSpeed =                0.075f;


    private Vector3 frictionDirection;        // apply curling

    private float FRICTION_MAX =                0.325f;
    private float FRICTION_MIN =                0.025f;
    private float COMMENTATOR_DELAY =           3.0f;

    private Vector3 BULLSEYE_POSITION;
        
    // --------------------------------------
    // functions
    // --------------------------------------
    void Start() {
		player =                    FindObjectOfType<Player>();
        inSupply =                  true;
        isPickedUp =                false;
        isFiring =                  false;
        hasJustFired =              false;
        frictionValue =             FRICTION_MAX;
        BULLSEYE_POSITION =         GameObject.FindGameObjectWithTag("Bullseye").transform.position;
        sphereCollider =            GetComponent<SphereCollider>();
    }

    void Update() {
        UpdateFriction();               //this must be first
		UpdateCamera();
        UpdateCommentator();
        StopMovingSlow();
    }

    public float DistanceFromBullseye() {
        return (transform.position - BULLSEYE_POSITION).magnitude;
    }

    public bool IsBeyondHouse() {
        return ((transform.position.z - GameManager.Singleton().BACK_OF_HOUSE_POSITION.z) > 0);
    }

    public bool IsBeforeGuardLine() {
        return ((transform.position.z - GameManager.Singleton().GUARD_LINE_POSITION.z) < 0);
    }

    public bool HasBeenFired() {
        return (!inSupply && !isPickedUp);
    }

    public void Pickup() {
        inSupply =              false;
        isPickedUp =            true;
    }

	public bool IsFiring() {
		return isFiring;
	}


    public void Fire() {
        isPickedUp =            false;
        isFiring =              true;
        hasJustFired =          true;
    }

	private void PlayCommentatorSound() {
		if (audio.isPlaying) {
			return;
		}

		audio.clip = commentatorSounds[ Random.Range( 0, commentatorSounds.Length ) ];
		audio.PlayDelayed(COMMENTATOR_DELAY);
	}

    private void UpdateCommentator() {
        if (isFiring) {
            PlayCommentatorSound();
        }
    }

    public bool InSupply() {
        return inSupply;
    }
    
    public bool IsPickedUp() {
        return isPickedUp;
    }

    public bool IsMoving() {
        return (rigidbody.velocity.magnitude > slowestSpeed);
    }

    public void StopMovingSlow() {
        if (rigidbody.velocity.magnitude > 0 && !IsMoving()) {
            rigidbody.velocity = Vector3.zero;
        }
    }

    private void UpdateFriction() {
        if (HasBeenFired() && GameManager.Singleton().GetGameState() == GameManager.eGameState.eRock) {
            rigidbody.AddForce(frictionDirection);

            if (rigidbody.velocity.magnitude > 3.2f) {
                rigidbody.AddForce(rigidbody.velocity * frictionValue * -1f);
            } else {
                rigidbody.AddForce(rigidbody.velocity * frictionValue * -2f);
            }
        }
    }

    // Send a value 0-1 to this function to set the friction value 
    // (0 = lowest, 1 = highest)
    public void SetFriction(float frictionPercent, Brusher.eScrubPlace scrubPlace) {
        frictionValue = frictionPercent * (FRICTION_MAX - FRICTION_MIN);
        
        if (frictionValue < FRICTION_MIN) {
            frictionValue = FRICTION_MIN;
        } else if (frictionValue > FRICTION_MAX) {
            frictionValue = FRICTION_MAX;
        }

        switch(scrubPlace) {
            case Brusher.eScrubPlace.eFarLeft:{
                frictionDirection = new Vector3(-0.05f * rigidbody.velocity.magnitude, 0f, 0f);
                break;
            }
            case Brusher.eScrubPlace.eLeft:{
                frictionDirection = new Vector3(-0.025f * rigidbody.velocity.magnitude, 0f, 0f);
                break;
            }
            case Brusher.eScrubPlace.eMiddle:{
                frictionDirection = Vector3.zero;
                break;
            }
            case Brusher.eScrubPlace.eRight:{
                frictionDirection = new Vector3(0.025f * rigidbody.velocity.magnitude, 0f, 0f);
                break;
            }
            case Brusher.eScrubPlace.eFarRight:{
                frictionDirection = new Vector3(0.05f * rigidbody.velocity.magnitude, 0f, 0f);
                break;
            }
        }

        frictionDirection *= frictionPercent;
    }

    private void UpdateCamera() {
        if (isFiring && !hasJustFired) {
            if (!IsMoving()) {
                player.StoneFired();
                isFiring = false;
            }
        }
    }

	private void OnCollisionEnter( Collision col ) {
		if (col.gameObject.GetType() == typeof(Rock)) {
            AudioSource.PlayClipAtPoint ( rockCollision ,transform.position );
		}
	}

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void CanSwitchCameraBack() {
        hasJustFired = false;
    }
}