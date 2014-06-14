/*
	FILE:			Player.cs
	AUHTOR:			Dan, Krz
	PROJECT:		Geri-Lynn Ramsey's Xtreme Curling 2014
	SOUNDTRACK:		Armin van Buuren Feat. Rank 1 & Kush - This world is watching me

	DESCRIPTION:	The player.

					Can be a brusher or a skip. Shoot stones and brushes.
*/

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public GameManager.eTeam team;
    private GameManager.eTeam teamPrev;
    public Camera rockCamera;
    public Camera playerCamera;
    
    
	private float speed =							0.0f;
	private float acceleration =					0.02f;
	private const float MAX_SPEED =					0.06f;
	private float sensitivity =						6.0f;
    private float frictionValue =                   0.2f;
    private float slowestSpeed =                    0.075f;
    private float maxLookAngle =                    10f;
    private bool canShoot =                         true;
	private bool canControl =						true;
	private bool passedTheLine =					false;
    private float dx;

	private Rock stoneClone;

    private int DEFAULT_FORCE =                     85;

    private Vector3 PLAYER_DEFAULT_POSITION =       new Vector3(0f, 1.5f, -61.5f);
    private Vector3 ROCK_CAMERA_DEFAULT_POSITION =  new Vector3(0f, 5f, -3f);
    private Vector3 HOGLINE_POSITION =              Vector3.zero;
    private float BOUNDARY_RESTRICTION_X_OFFSET =   7f;
    private float BOUNDARY_RESTRICTION_Z_OFFSET =   34f;
    private int DELAY_BETWEEN_CAMERA_SWITCH =       3;

	void Start() {
	    HOGLINE_POSITION =               GameObject.FindGameObjectWithTag("Hogline").transform.position;
        ROCK_CAMERA_DEFAULT_POSITION =   PLAYER_DEFAULT_POSITION + ROCK_CAMERA_DEFAULT_POSITION;
        transform.position =             PLAYER_DEFAULT_POSITION;
        
        SwitchState(GameManager.eGameState.ePlayer);
        GiveStone();
	}

    void Update() {
        UpdateStone();            // this needs to go first
        ResetIfOutOfBounds();
		Move();
		Look();
        UpdateFriction();
        UpdateAnimation();
	}

	public void Move() {
		if ( canControl ) {
			float dz =				Input.GetAxis( "Vertical" );
			dz =					Mathf.Clamp( dz, -(speed * 0.5f), speed );

            Vector3 direction = new Vector3(0f, 0f, dz);

            if (CanMoveInDirection(direction)) {
                direction = transform.TransformDirection(direction);
                // move player
                rigidbody.AddForce(direction * DEFAULT_FORCE);
            }

            if (HOGLINE_POSITION.z - transform.position.z < 0) {
                passedTheLine = true;
                Disqualify();
            }

            // only apply accel while below max speed and pressing movement keys
			if ( speed < MAX_SPEED && MovementKeysPressed() ) {
				speed += acceleration;
			}
		}
	}

    private void UpdateAnimation() {
        if (IsMoving()) {
            animation.CrossFade( "Running" );
        } else {
            animation.Play( "Idle" );
            animation.CrossFade( "Idle" );
        }
    }

	public void Look() {
		dx += Input.GetAxis( "Mouse X" ) * sensitivity;
        dx = Mathf.Clamp( dx, -maxLookAngle, maxLookAngle );
        transform.Rotate( 0f, -dx, 0f );
        transform.rotation = Quaternion.Euler( transform.rotation.x, dx + transform.rotation.y, transform.rotation.z );
	}
    
    public void GiveStone() {
        bool found = false;

        RespawnPlayer();
        
		foreach ( Rock stone in FindObjectsOfType<Rock>() ) {
			if ( stone.InSupply() && stone.team == team ) {
				stoneClone =					stone;
                stoneClone.transform.parent =   null;
                stoneClone.transform.position = transform.position + (transform.forward + transform.forward);
                ResetRockCamera();
                rockCamera.transform.parent =   stoneClone.transform;
                stone.Pickup();
                found = true;
				break;
			}
		}

        if ( found ) {
            canControl = true;
        } else {
            // in case there are stones left and the current player is not on the same team as the last stones
            SwitchTeam();
            GiveStone();
        }
	}

	public void UpdateStone() {
		if ( stoneClone.IsPickedUp() ) {
			stoneClone.transform.position = transform.position + ( transform.forward + transform.forward );
			if ( canShoot && IsMoving() ) {
				if ( Input.GetMouseButtonDown( 0 ) ) {
					ShootStone();
				}
			}
		}
	}

	public void ShootStone() {
		if ( !passedTheLine ) {
			canShoot = false;
			canControl = false;

			// apply our current velocity to the stone
			stoneClone.rigidbody.AddForce( rigidbody.velocity * DEFAULT_FORCE );

			SwitchState(GameManager.eGameState.eRock);     //switch to rockCamera which follows the stone
			stoneClone.Fire();                             //this will call StoneFired() when the stone stops moving
            StartCoroutine( AllowRockToPassCameraBack() );
    	}
	}

    private IEnumerator AllowRockToPassCameraBack() {
        yield return new WaitForSeconds(DELAY_BETWEEN_CAMERA_SWITCH);

        stoneClone.CanSwitchCameraBack();
    }

    // resets the stone after being fired
    public void StoneFired() {
        if ( StonesInSupply() > 0 ) {
        	passedTheLine = false;
            canShoot = true;
            GiveStone();
            EndOfTurn();
        } else {                        // if there are no stones in the supply then it must be the end of the game
            EndOfRound();
        }
    }

    private void EndOfTurn() {
        // if i've been the same team for the last 2 turns switch team
        if ( teamPrev == team ) {
            SwitchTeam();
        } else {
            teamPrev = team;
        }

        ClearUpBurnedStones();

        SwitchState(GameManager.eGameState.ePlayer);
    }

    private int StonesInSupply() {
        int i = 0;
        foreach ( Rock stone in FindObjectsOfType<Rock>() ) {
            if ( stone.InSupply() ) {
                i++;
            }
        }

        return i;
    }

	private void SwitchState( GameManager.eGameState state ) {
		GameManager.Singleton().ChangeState( state );
    }

    private void SwitchTeam() {
        teamPrev = team;

        switch ( team ) {
            case GameManager.eTeam.TEAM_RED:
            {
                team = GameManager.eTeam.TEAM_BLUE;
                break;
            }

            case GameManager.eTeam.TEAM_BLUE:
            {
                team = GameManager.eTeam.TEAM_RED;
                break;
            }
        }
    }

    private void ResetRockCamera() {
        rockCamera.transform.parent = null;
        //rockCamera.transform.position = Vector3.zero;
        
        rockCamera.transform.position = ROCK_CAMERA_DEFAULT_POSITION;
    }

    private void EndOfRound() {
        SwitchState(GameManager.eGameState.eBullseye);
        //reset game or load a scene to show the winner
    }

    private bool MovementKeysPressed() {
        return (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);
    }

    public void SetFriction(float friction) {
        stoneClone.SetFriction(friction);
    }

    public void ClearUpBurnedStones() {
        foreach ( Rock stone in FindObjectsOfType<Rock>() ) {
            if ( stone.HasBeenFired() ) {
                if ( stone.IsBeyondHouse() || stone.IsBeforeGuardLine() ) {
                    //possibly add something cool here like an explosion at (stone.transform.position + Vector3(0f, 1f, 0f))
                    stone.transform.position = (stone.transform.position + new Vector3(0f, -50f, 0f));
                    stone.renderer.enabled =        false;
                    stone.enabled =                 false;
                    stone.sphereCollider.enabled =  false;
                    Destroy(stone);
                }
            }
        }
    }

    private void UpdateFriction() {
        // stop moving if im barely moving anyway
        if (rigidbody.velocity.magnitude > 0 && !IsMoving()) {
            rigidbody.velocity = Vector3.zero;
        }

        if (IsMoving()) {
            rigidbody.AddForce(rigidbody.velocity * frictionValue * -1f);
        }

        if (speed >= (acceleration * 2f) && !MovementKeysPressed()) {
            speed -= acceleration;
        }
    }

    public bool IsMoving() {
        return (rigidbody.velocity.magnitude > slowestSpeed);
    }

    public void Disqualify() {
        // tell player they have been disqualified
        GameManager.Singleton().HUDDisqualified();
        canControl = false;
        canShoot = false;
        
        StartCoroutine( DisqualifyCont() );
    }

    IEnumerator DisqualifyCont() {
        yield return new WaitForSeconds(1);

        float disqualifyOffset = GameManager.Singleton().BACK_OF_HOUSE_POSITION.z - 1.0f;
        Vector3 pos = stoneClone.transform.position;
        stoneClone.transform.position = new Vector3(pos.x, pos.y, disqualifyOffset);
        StoneFired();
    }


    private bool CanMoveInDirection(Vector3 direction) {
        Vector3 newPosition = transform.position + direction;

        // left boundary restriction
        if (newPosition.x < (HOGLINE_POSITION.x - BOUNDARY_RESTRICTION_X_OFFSET)) {
            return false;
        }

        // right boundary restriction
        if (newPosition.x > (HOGLINE_POSITION.x + BOUNDARY_RESTRICTION_X_OFFSET)) {
            return false;
        }

        // nearest boundary restriction
        if (newPosition.z < (HOGLINE_POSITION.z - BOUNDARY_RESTRICTION_Z_OFFSET)) {
            return false;
        }


        return true;
    }

    private void RespawnPlayer() {
        transform.position = PLAYER_DEFAULT_POSITION;
        transform.rotation = Quaternion.identity;
        rigidbody.velocity = Vector3.zero;
    }

    private void ResetIfOutOfBounds() {
        // if player is already out of bounds disqualify them && "Z-kill"
        if ( !CanMoveInDirection(Vector3.zero) || transform.position.y < -10 ) {
            RespawnPlayer();

            // tell player their position was reset
            GameManager.Singleton().HUDResetPosition();
        }
    }

    public GameManager.eTeam GetTeam() {
        return stoneClone.team;
    }
}