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
	private float acceleration =					0.05f;
	private const float MAX_SPEED =					0.02f;
	private float sensitivity =						6.0f;
    private float frictionValue =                   0.2f;
    private float slowestSpeed =                    0.075f;
    private bool canShoot =                         true;
	private bool canControl =						true;

	private Rock stoneClone;

    private int DEFAULT_FORCE =                     85;

    private Vector3 PLAYER_DEFAULT_POSITION =       new Vector3(0f, 1.5f, -61.5f);
    private Vector3 ROCK_CAMERA_DEFAULT_POSITION =  Vector3.zero;
    private Vector3 ROCK_CAMERA_DEFAULT_ROTATION =	new Vector3( 30.0f, 0.0f, 0.0f );
    private Vector3 HOGLINE_POSITION =              Vector3.zero;
    private Vector3 STONE_SPAWN_OFFSET =            new Vector3(0f, -1f, 2.5f);

	void Start() {
        ROCK_CAMERA_DEFAULT_POSITION =   new Vector3( 0.0f, PLAYER_DEFAULT_POSITION.y + 7f, PLAYER_DEFAULT_POSITION.z - 10f );
        HOGLINE_POSITION =               GameObject.FindGameObjectWithTag("Hogline").transform.position;
        
        SwitchCamera(GameManager.eGameState.ePlayer);
        GiveStone();
	}

    void Update() {
        UpdateStone();      // this needs to go first
		Move();
		Look();
        UpdateFriction();
        UpdateAnimation();

		if ( speed >= (acceleration * 2f) && !MovementKeysPressed() ) {
		 	speed -= acceleration;
		}
	}

	public void Move() {
		if ( canControl ) {
			//float dx =				Input.GetAxis( "Horizontal" );
			float dz =				Input.GetAxis( "Vertical" );

			//dx =					Mathf.Clamp( dx, -speed, speed );
			dz =					Mathf.Clamp( dz, -(speed * 0.5f), speed );

            Vector3 direction =		new Vector3( 0f, 0f, dz );
			direction =				transform.TransformDirection( direction );
            
            // move player
			rigidbody.AddForce( direction * DEFAULT_FORCE );

            // only apply accel while below max speed and pressing movement keys
			if ( speed < MAX_SPEED && MovementKeysPressed() ) {
				speed += acceleration;
			}
		}
	}

    private void UpdateAnimation() {
        if (speed > 0.01) {
            //animation.CrossFade( "Running" );
        } else {
            //animation.Play( "Idle" );
        }
    }


	public void Look() {
		float dy = Input.GetAxis( "Mouse X" ) * sensitivity;     
		transform.Rotate( 0f, -dy, 0f );
	}
    
    public void GiveStone() {
        bool found = false;

        transform.position = PLAYER_DEFAULT_POSITION;
        transform.rotation = Quaternion.identity;
        
		foreach ( Rock stone in FindObjectsOfType<Rock>() ) {
			if ( stone.InSupply() && stone.team == team ) {
				stoneClone =					stone;
				rockCamera.transform.parent =	stoneClone.transform;
                ResetRockCamera();
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
		if ( stoneClone.IsPickedUp() && canShoot ) {
			stoneClone.transform.position = transform.position + transform.forward + transform.forward + transform.forward;
			if ( Input.GetMouseButtonDown( 0 ) ) {
				ShootStone();
			}
		}
	}

    public void ShootStone() {
        canShoot = false;
        canControl = false;
        //stoneClone.transform.parent = null;

        SwitchCamera(GameManager.eGameState.eRock);     //switch to rockCamera which follows the stone

        // apply our current velocity to the stone
        stoneClone.rigidbody.velocity = rigidbody.velocity;
        //stoneClone.rigidbody.AddForce( rigidbody.velocity * DEFAULT_FORCE );

        stoneClone.Fire();                              //this will call StoneFired() when the stone stops moving
	}

    public void StoneFired() {
        if ( StonesInSupply() > 0 ) {
            canShoot = true;
            GiveStone();
            EndOfTurn();
        } else {                        // if there are no stones in the supply then it must be the end of the game
            EndOfRound();
        }
    }

    private void EndOfTurn() {
        // if i've been the same team for the last 2 turns switch team
        if ( teamPrev == team )
        {
            SwitchTeam();
        } else {
            teamPrev = team;
        }

        ClearUpBurnedStones();

        SwitchCamera(GameManager.eGameState.ePlayer);
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

	private void SwitchCamera( GameManager.eGameState state ) {
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
        rockCamera.transform.position = ROCK_CAMERA_DEFAULT_POSITION;
        rockCamera.transform.rotation = Quaternion.Euler( ROCK_CAMERA_DEFAULT_ROTATION );
    }

    private void EndOfRound() {
        SwitchCamera(GameManager.eGameState.eBullseye);
        GameManager.Singleton().UpdateScores();
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
            if ( stone.IsBeyondHouse() ) {
                //possibly add something cool here like an explosion at (stone.transform.position + Vector3(0f, 1f, 0f))
                Destroy(stone);
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
    }

    public bool IsMoving() {
        return (rigidbody.velocity.magnitude > slowestSpeed);
    }
}