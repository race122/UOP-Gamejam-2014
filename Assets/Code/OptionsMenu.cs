using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

	private float newVolume;

	// Use this for initialization
	void Start () {
        newVolume = GameManager.GetVolume() * 100;
		Debug.Log (newVolume);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI() {
		//newVolume = GUI.HorizontalSlider(new Rect(  ( ( Screen.width * 0.5f) * 0.7f ), ( Screen.height * 0.5f ) , 300, 150), newVolume, 0.0F, 100.0F);
		newVolume = GUI.HorizontalSlider(new Rect(  ( ( Screen.width * 0.5f) - 150), ( Screen.height * 0.5f ) , 300, 150), newVolume, 0.0F, 100.0F);

        GameManager.SetVolume(newVolume);

		//GUI.Label( new Rect( ( ( Screen.width * 0.5f ) * 0.95f ), ( ( Screen.height * 0.5f ) * 0.9f ) , 300, 25 ), "Volume: " + Mathf.Round( newVolume ) );
		GUI.Label( new Rect( ( ( Screen.width * 0.5f ) * 0.95f ), ( ( Screen.height * 0.5f ) - 25 ) , 300, 25 ), "Volume: " + Mathf.Round( newVolume ) );

	}
}
