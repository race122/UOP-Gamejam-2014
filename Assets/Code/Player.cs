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
    public Camera rockCamera;
    public Camera playerCamera;

	private float speed =							0.0f;
	private float acceleration =					0.05f;
	private const float MAX_SPEED =					0.5f;
	private float sensitivity =						12.0f;
	private bool canShoot =							true;
	private bool canControl =						true;

	private Vector3 cameraToPlayerOffset;
	private Rock stoneClone;

    private int DEFAULT_FORCE =                     100;
	private Vector3 DEFAULT_PLAYER_POSITION =		new Vector3( 0f, 1f, -22f );
    private Vector3 ROCK_CAMERA_DEFAULT_POSITION =  Vector3.zero;
    private Vector3 ROCK_CAMERA_DEFAULT_ROTATION =	new Vector3( 30.0f, 0.0f, 0.0f );

	void Start() {
        ROCK_CAMERA_DEFAULT_POSITION =   new Vector3( 0.0f, DEFAULT_PLAYER_POSITION.y + 3f, DEFAULT_PLAYER_POSITION.z - 2.5f );
		cameraToPlayerOffset =           new Vector3( transform.position.x + 8, transform.position.y + 6, transform.position.z + 4 );

		GiveStone();
        animation.Play( "Idle" );
	}

	void Update() {
		Move();
		Look();
		UpdateStone();
		// ShootStone();

		if ( speed >= (acceleration * 2f) && !MovementKeysPressed() ) {
		 	speed -= acceleration;
		}

		if ( playerCamera.transform.parent == transform ) {
			camera.transform.position =	cameraToPlayerOffset;
			camera.transform.rotation =	Quaternion.Euler( 30f, -90f, 0f );
		}
	}

	public void Move() {
		if ( canControl ) {
			float dx =				Input.GetAxis( "Horizontal" );
			float dz =				Input.GetAxis( "Vertical" );

			dx =					Mathf.Clamp( dx, -speed, speed );
			dz =					Mathf.Clamp( dz, -speed, speed );

			Vector3 direction =		new Vector3( dx, 0f, dz );
			direction =				transform.TransformDirection( direction );

            // move player
			rigidbody.AddForce( direction * DEFAULT_FORCE );

			if ( speed < MAX_SPEED && MovementKeysPressed() ) {
				speed += acceleration;
                animation.CrossFade( "Running" );
			}
		}
	}

	public void Look() {
		float dy = Input.GetAxis( "Mouse Y" ) * sensitivity;        
		transform.Rotate( 0f, -dy, 0f );
	}

    /*public void Look()
    {
        transform.rotation = Quaternion.Euler(0f, Input.mousePosition.y, 0f);
    }*/

    public void GiveStone() {
        bool found = false;

        transform.position = DEFAULT_PLAYER_POSITION;
        transform.rotation = Quaternion.identity;

        Vector3 clonePos = transform.position;
        clonePos.z += 1.5f;
		foreach ( Rock stone in FindObjectsOfType<Rock>() ) {
			if ( stone.InSupply() && stone.team == team ) {
				stoneClone =					stone;
				stone.transform.position =		clonePos;
				//stoneClone.transform.parent =	transform;
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
		if ( stoneClone.IsPickedUp() ) {
			stoneClone.transform.position = transform.position + ( transform.forward + transform.forward );
			if ( Input.GetMouseButtonDown( 0 ) && canShoot ) {
				ShootStone();
			}
		}
	}

	public void ShootStone() {
		stoneClone.transform.parent = null;

        SwitchCamera( GameManager.eGameState.eRock );
        Vector3 forwardForce =			rigidbody.velocity;
        stoneClone.rigidbody.AddForce(forwardForce * DEFAULT_FORCE);

        stoneClone.Fire();
        canShoot = false;
        canControl = false;
	}

    public void StoneFired() {
        if ( StonesInSupply() > 0 ) {
            SwitchTeam();
            GiveStone();
            canShoot = true;
        } else {
            EndOfRound();
        }

        SwitchCamera( GameManager.eGameState.ePlayer );
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
        GameManager.Singleton().UpdateScores();
        //reset game or load a scene to show the winner
    }

    private bool MovementKeysPressed() {
        return (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);
    }

    public void SetFriction(float friction) {
        stoneClone.SetFriction(friction);
    }
}