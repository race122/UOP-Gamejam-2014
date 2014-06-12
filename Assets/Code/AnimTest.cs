using UnityEngine;
using System.Collections;

public class AnimTest : MonoBehaviour {
	void Start() {
	}
	
	void Update() {
		if ( Input.GetKey( KeyCode.W ) ) {
			animation.CrossFade( "Running", 0.35f );
		}
	}
}