using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {
	
	public string itemName;
	public GameObject menuOutline;
	public AudioClip mouseOver;
	public AudioClip mouseClick;
	public AudioClip mouseBack;
	
	// Use this for initialization
	void Start () {
		Screen.lockCursor = false;
		Screen.showCursor = true;
		
		if (PlayerPrefs.HasKey ("volume")) {
						AudioListener.volume = GameManager.GetVolume ();
			
				} else {
			GameManager.SetVolume( 1.0f );
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	//Detect when mouse hovers over menu item
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
