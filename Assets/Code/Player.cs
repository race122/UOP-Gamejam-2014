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
	public GameObject camera;
    public Camera rockCamera;

	private float speed =							0.0f;
	private float acceleration =					0.0005f;
	private const float MAX_SPEED =					5.0f;
	private float shootSpeed =						25.0f;
	private float sensitivity =						12.0f;
	private bool canShoot =							true;
	private bool canControl =						true;

	private Vector3 cameraToPlayerOffset;
	private Rock stoneClone;

	private Vector3 DEFAULT_PLAYER_POSITION =		new Vector3( 0f, 1f, 0f );
    private Vector3 ROCK_CAMERA_DEFAULT_POSITION =	new Vector3( 0.0f, 4.0f, -5.5f );
    private Vector3 ROCK_CAMERA_DEFAULT_ROTATION =	new Vector3( 30.0f, 0.0f, 0.0f );

	void Start() {
		cameraToPlayerOffset = new Vector3( transform.position.x + 8, transform.position.y + 6, transform.position.z + 4 );

		GiveStone();
	}

	void Update() {
		Move();
		Look();
		UpdateStone();
		// ShootStone();

		// if ( speed > 0 ) {
		// 	speed -= acceleration;
		// }

		if ( camera.transform.parent == transform ) {
			camera.transform.position =	cameraToPlayerOffset;
			camera.transform.rotation =	Quaternion.Euler( 30f, -90f, 0f );
		}
	}

	public void Move() {
		if ( canControl ) {
			float dx =				Input.GetAxis( "Horizontal" ) * speed;
			float dz =				Input.GetAxis( "Vertical" ) * speed;

			dx =					Mathf.Clamp( dx, -speed, speed );
			dz =					Mathf.Clamp( dz, -speed, speed );

			Vector3 direction =		new Vector3( dx, 0f, dz );
			direction =				transform.TransformDirection( direction );

			// rigidbody.AddForce( direction );
			transform.Translate( direction, Space.World );

			if ( speed < MAX_SPEED ) {
				speed += acceleration;
			}
		}
	}

	public void Look() {
		float dy = Input.GetAxis( "Mouse Y" ) * sensitivity;
		transform.Rotate( 0f, -dy, 0f );
	}

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
		//stoneClone.transform.parent = null;

        SwitchCamera( GameManager.eGameState.eRock );
		
        Vector3 forwardForce =			transform.forward;
        
        forwardForce.x *= rigidbody.velocity.magnitude * 100f;
        forwardForce.y *= rigidbody.velocity.magnitude * 100f;
        forwardForce.z *= rigidbody.velocity.magnitude * 100f;
		//stoneClone.rigidbody.velocity = rigidbody.velocity;

        stoneClone.rigidbody.AddForce(forwardForce);


        stoneClone.Fire();
        canShoot = false;
        canControl = false;

        // must be at the end
        StartCoroutine( StoneFired() );
	}

    private IEnumerator StoneFired() {
        yield return new WaitForSeconds( 3 );
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

        print( i );

        return i;
    }

    private int TeamOneStonesLeft() {
        int i = 0;

        foreach ( Rock stone in FindObjectsOfType<Rock>() ) {
            if ( stone.InSupply() && stone.team == GameManager.eTeam.TEAM_1 ) {
                i++;
            }
        }

        print( i );

        return i;
    }

    private int TeamTwoStonesInSupply() {
        int i = 0;

        foreach ( Rock stone in FindObjectsOfType<Rock>() ) {
            if ( stone.InSupply() && stone.team == GameManager.eTeam.TEAM_2 ) {
                i++;
            }
        }

        print( i );

        return i;
    }

	private void SwitchCamera( GameManager.eGameState state ) {
		GameManager.Singleton().ChangeState( state );
    }

    private void SwitchTeam() {
        switch ( team ) {
            case GameManager.eTeam.TEAM_1:
            {
                team = GameManager.eTeam.TEAM_2;
                break;
            }

            case GameManager.eTeam.TEAM_2:
            {
                team = GameManager.eTeam.TEAM_1;
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
}