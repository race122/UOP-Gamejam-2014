using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
	private float cameraSpeed = -1.0f;

	void Start() {
	}
	
	void Update() {
		if ( Input.GetKeyDown( KeyCode.Escape ) ) {
			Application.LoadLevel( "MainMenu" );
		}

		//camera.transform.translate( 0f, cameraSpeed, 0f );
	}
}