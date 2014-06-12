using UnityEngine;
using System.Collections;

public class Player2 : MonoBehaviour {
	private bool canControl = true;
	private bool isMoving = false;
	private float speed = 0.0f;
	private float accel = 0.005f;
	private float decel = 0.0025f;
	private const float MAX_SPEED = 0.1f;

	void Start() {
		
	}
	
	float dx, dz;

	void Update() {
		if ( canControl ) {
			if ( Input.GetKey( KeyCode.W ) ) {
				dz += accel;
			}

			if ( Input.GetKey( KeyCode.S ) ) {
				dz -= accel;
			}

			if ( Input.GetKey( KeyCode.A ) ) {
				dx -= accel;
			}

			if ( Input.GetKey( KeyCode.D ) ) {
				dx += accel;
			}

			print( speed );
		}

		// I feel dirty...
		if ( Input.GetKey( KeyCode.W ) || Input.GetKey( KeyCode.A ) || Input.GetKey( KeyCode.S ) || Input.GetKey( KeyCode.D ) ) {
			if ( speed < MAX_SPEED ) {
				speed += accel;
			}
		} else {
			if ( rigidbody.velocity.magnitude > 0 && speed > 0 ) {
				speed -= decel;
			}
		}

		Vector3 direction = new Vector3( dx * speed, 0f, dz * speed );
		transform.Translate( direction );
	}
}