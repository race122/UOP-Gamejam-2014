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
    private float frictionValue;                // a value between friction max and min effectivly acts as a percentage


    private Vector3 curlingDirection;                                      // curling direction
    private float curlingPercent;                                          // how much

    private static float FRICTION_MAX =                 40.0f;
    private static float FRICTION_MIN =                 10.0f;
    private static float FRICTION_FACTOR_HIGH_SPEED =   .550f;
    private static float FRICTION_MED_SPEED =           4.80f;              //above this is high speed
    private static float FRICTION_FACTOR_MED_SPEED =    0.70f;
    private static float FRICTION_LOW_SPEED =           .500f;
    private static float FRICTION_FACTOR_LOW_SPEED =    2.00f;
    private static float FRICTION_STOP_SPEED =          .010f;              //if im going this slow just stop me moving
    private static float FRICTION_CURLING_FACTOR =      10.0f;
    private static float COMMENTATOR_DELAY =            3.00f;

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

    // this is where all the forces are applied
    private void UpdateFriction() {
        // this excludes all stones which have not been pulled into the game yet
        if (HasBeenFired() && GameManager.Singleton().GetGameState() == GameManager.eGameState.eRock) {
            // this is the curling
            // apply force proportional to the amount of brushing
            if (curlingDirection.magnitude > 0f) {
                Vector3 force = curlingDirection;                       // take the friction direction (movement on X only)
                force *= Time.deltaTime;                                // scale by delta time - ensures the same results on all systems
                force *= (rigidbody.velocity.magnitude * 0.25f);        // scale by quarter of the speed we're currently going
                if (rigidbody.velocity.magnitude > FRICTION_MED_SPEED) {    // if velocity is in high speed
                    force *= (FRICTION_CURLING_FACTOR * 0.50f);            // scale by half of the curling factor
                } else {                                                    // else
                    force *= FRICTION_CURLING_FACTOR;                   // scale by the curling factor
                }
                force *= curlingPercent;                                // scale by the curling percentage
                rigidbody.AddForce(force);                              // then apply the force
            }


            // this is the basic friction
            if (rigidbody.velocity.magnitude > FRICTION_MED_SPEED) {           
                //brushing should not have as much of an influence at max speed
                // BRUSH_IGNORE_PERCENT% of max friction is enforced, 1f-BRUSH_IGNORE_PERCENT% is made from frictionValue
                Vector3 force = -rigidbody.velocity;                    // take the current velocity and reverse it
                force *= FRICTION_MAX;                                  // apply maximum friction at this speed, ignore the frictionValue
                force *= FRICTION_FACTOR_HIGH_SPEED;                   // scale by the high speed factor
                force *= Time.deltaTime;
                rigidbody.AddForce(force);

            } else if (rigidbody.velocity.magnitude > FRICTION_LOW_SPEED) {    
                //this is the standard speed when you would expect the player to brush
                rigidbody.AddForce(-rigidbody.velocity * frictionValue * Time.deltaTime * FRICTION_FACTOR_MED_SPEED);

            } else {   
                // this is to counter act them getting slower and slower exponentially
                rigidbody.AddForce(-rigidbody.velocity * frictionValue * Time.deltaTime * FRICTION_FACTOR_LOW_SPEED);
            }
        }
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
        return (rigidbody.velocity.magnitude > FRICTION_STOP_SPEED);
    }

    public void StopMovingSlow() {
        if (rigidbody.velocity.magnitude > 0 && !IsMoving()) {
            rigidbody.velocity = Vector3.zero;
        }
    }

    // Send a value 0-1 to this function to set the friction value 
    // (0 = lowest, 1 = highest)
    public void SetFriction(float frictionPercent, Brusher.eScrubPlace scrubPlace) {
        curlingPercent = 1f - frictionPercent;                              // friction and curling are inverse
        frictionValue = frictionPercent * (FRICTION_MAX - FRICTION_MIN);
        
        if (frictionValue < FRICTION_MIN) {
            frictionValue = FRICTION_MIN;
        } else if (frictionValue > FRICTION_MAX) {
            frictionValue = FRICTION_MAX;
        }

        switch(scrubPlace) {
            case Brusher.eScrubPlace.eFarLeft:{
                curlingDirection = new Vector3(-0.35f * rigidbody.velocity.magnitude, 0f, 0f);
                break;
            }
            case Brusher.eScrubPlace.eLeft:{
                curlingDirection = new Vector3(-0.25f * rigidbody.velocity.magnitude, 0f, 0f);
                break;
            }
            case Brusher.eScrubPlace.eMiddle:{
                curlingDirection = Vector3.zero;
                break;
            }
            case Brusher.eScrubPlace.eRight:{
                curlingDirection = new Vector3(0.25f * rigidbody.velocity.magnitude, 0f, 0f);
                break;
            }
            case Brusher.eScrubPlace.eFarRight:{
                curlingDirection = new Vector3(0.35f * rigidbody.velocity.magnitude, 0f, 0f);
                break;
            }
        }

        curlingDirection *= curlingPercent;
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