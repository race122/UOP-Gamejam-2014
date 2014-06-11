﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public GameManager.eTeam team;
	public GameObject camera;
    public Camera rockCamera;

	private float speed =			0.0f;
	private float acceleration =	0.0005f;
	private const float MAX_SPEED =	50.0f;
	private float shootSpeed =		25.0f;
	private float sensitivity =		4.2f;
	private bool canShoot =			true;
	private bool canControl =		true;

	private Rock stoneClone;

    private Vector3 ROCK_CAMERA_DEFAULT_POSITION = new Vector3(0.0f, 4.0f, -5.5f);
    private Vector3 ROCK_CAMERA_DEFAULT_ROTATION = new Vector3(30.0f, 0.0f, 0.0f);

	void Start() {
		GiveStone();
	}

	void Update() {
		Move();
		Look();
		ShootStone();

		// if ( speed > 0 ) {
		// 	speed -= acceleration;
		// }

		if ( camera.transform.parent == transform ) {
			camera.transform.position =	new Vector3( transform.position.x + 8, transform.position.y + 6, transform.position.z + 4 );
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
    /*
	public void GiveStone() {
		Vector3 clonePos = transform.position;
		clonePos.z += 1.5f;

		stoneClone = (GameObject) Instantiate( rock, clonePos, Quaternion.identity );
		stoneClone.transform.parent = transform;
	}*/

    public void GiveStone() 
    {
        Vector3 clonePos = transform.position;
        clonePos.z += 1.5f;

        foreach (Rock stone in FindObjectsOfType<Rock>())
        {
            if (stone.InSupply() && stone.team == team)
            {
                stoneClone = stone;
                stone.transform.position = clonePos;
                stoneClone.transform.parent = transform;
                rockCamera.transform.parent = stoneClone.transform;
                ResetRockCamera();
                break;
            }
        }
        
    }

	public void ShootStone() {
		if ( Input.GetMouseButtonDown( 0 ) && canShoot ) {
			canShoot =						false;
			canControl =					false;
			stoneClone.transform.parent =	null;
			//camera.transform.parent =		stoneClone.transform;
			Vector3 forwardForce =			transform.forward;
            
            SwitchCamera(GameManager.eGameState.eRock);
            SwitchTeam();
			
			forwardForce *= ( shootSpeed * shootSpeed );
			stoneClone.rigidbody.AddForce( forwardForce );
            stoneClone.Fire();

       		StartCoroutine( StoneFired() );      
		}
	}

    private int StonesInSupply()
    {
        int i = 0;

        foreach (Rock stone in FindObjectsOfType<Rock>())
        {
            if (stone.InSupply())
            {
                i++;
            }
        }

        print(i);

        return i;
    }

    private IEnumerator StoneFired() {
        yield return new WaitForSeconds( 3 );
		if (StonesInSupply() > 0)
        {
            GiveStone();
            SwitchCamera( GameManager.eGameState.ePlayer );
            canShoot = true;
        }
    }

    private void SwitchCamera(GameManager.eGameState state) {
        GameManager.Singleton().ChangeState(state);
    }

    private void SwitchTeam() {
        switch (team)
        {
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
}