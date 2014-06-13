/*
	FILE:			Credits.cs
	AUHTOR:			Dan
	PROJECT:		Geri-Lynn Ramsey's Xtreme Curling 2014
	SOUNDTRACK:		Custom Carnival Mix - http://www.mixcloud.com/floormanuk/carnival-style-mix/

	DESCRIPTION:	Hey Pete! I can do fancy credits screens too!
*/
using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
	public GameObject credits;
	public SpriteRenderer finalImage;

	private float cameraSpeed = 0.005f;
	private Vector3 finalImageLastPos = new Vector3( 0f, 1f, 0f );

	void Start() {
	}
	
	void Update() {
		if ( Input.GetKeyDown( KeyCode.Escape ) ) {
			Application.LoadLevel( "MainMenu" );
		}

		credits.transform.Translate( 0f, cameraSpeed, 0f );
		if ( finalImage.transform.position.y >= 1 ) {
			finalImage.transform.position = finalImageLastPos;
			StartCoroutine( FadeOut() );
		}
	}

	IEnumerator FadeOut() {
		yield return new WaitForSeconds( 5 );

		float alpha = finalImage.color.a;
		//alpha += ( 0.0f - alpha ) * 0.02f;
		alpha -= 0.02f;

		finalImage.color = new Color( 1f, 1f, 1f, alpha );

		//print( alpha );

		if ( alpha <= 0 ) {
			alpha = 0;
			yield return new WaitForSeconds( 1 );
			Application.LoadLevel( "MainMenu" );
		}
	}
}