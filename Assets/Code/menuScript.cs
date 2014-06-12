using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public string itemName;
	public GameObject menuOutline;
	public AudioClip mouseOver;
	public AudioClip mouseClick;
	public AudioClip mouseBack;

	// Use this for initialization
	void Start () {
<<<<<<< HEAD
		if ( PlayerPrefs.GetFloat("volume") < 0.0f )
		{
			GameManager.setVolume( 1.0f );
			Debug.Log ("Volume was reset");
		}

		AudioListener.volume = GameManager.getVolume();
=======
		Screen.lockCursor = false;
        Screen.showCursor = true;

		if ( !PlayerPrefs.HasKey("volume") {
			GameManager.Singleton().setVolume( 1.0f );
			Debug.Log ("Volume was reset");
		}

        AudioListener.volume = GameManager.Singleton().getVolume();
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUp()
	{
		if ( itemName == "start game" )
		{
			Debug.Log("Start Game");
			Application.LoadLevel("dansTest");
		}

		else if ( itemName == "options" )
		{
			Debug.Log("Options");
			Application.LoadLevel("optionsMenu");
		}

		else if ( itemName == "credits" )
		{
			Debug.Log("Credits");
			Application.LoadLevel("creditsMenu");
		}

		else if ( itemName == "exit" )
		{
			Debug.Log("Exit");
			Application.Quit();
		}

		else if ( itemName == "back" )
		{
			Debug.Log("back");
			Application.LoadLevel("mainMenu");
		}

		AudioSource.PlayClipAtPoint(mouseClick, transform.position);
	}

	//Mouse enter function
	void OnMouseEnter()
	{
		if ( itemName == "start game" )
		{
			Instantiate( menuOutline, transform.position, transform.rotation );
		}
		
		else if ( itemName == "options" )
		{
			Instantiate( menuOutline, transform.position, transform.rotation );
		}
		
		else if ( itemName == "credits" )
		{
			Instantiate( menuOutline, transform.position, transform.rotation );
		}
		
		else if ( itemName == "exit" )
		{
			Instantiate( menuOutline, transform.position, transform.rotation );
		}

		else if ( itemName == "back" )
		{
			Instantiate( menuOutline, transform.position, transform.rotation );
		}

		AudioSource.PlayClipAtPoint(mouseOver, transform.position);
	}

	void OnMouseExit()
	{
		Destroy( GameObject.FindWithTag("menuOutline") );
	}	
}
