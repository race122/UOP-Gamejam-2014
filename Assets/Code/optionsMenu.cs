using UnityEngine;
using System.Collections;

<<<<<<< HEAD
public class optionsMenu : MonoBehaviour {
=======
public class OptionsMenu : MonoBehaviour {
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b

	public float newVolume;

	// Use this for initialization
	void Start () {
<<<<<<< HEAD
		newVolume = GameManager.getVolume() * 100;
=======
        newVolume = GameManager.Singleton().getVolume() * 100;
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
		Debug.Log (newVolume);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI() {
		newVolume = GUI.HorizontalSlider(new Rect( ( Screen.width / 2 ) - 150, ( Screen.height / 2 ) , 300, 150), newVolume, 0.0F, 100.0F);

<<<<<<< HEAD
		GameManager.setVolume( ( newVolume ) );

		GUI.Label( new Rect( ( Screen.width / 2 ) - 50, ( Screen.height / 2 ) - 25 , 300, 25 ), "Volume: " + newVolume);
=======
        GameManager.Singleton().setVolume((newVolume));

		GUI.Label( new Rect( ( Screen.width / 2 ) - 40, ( Screen.height / 2 ) - 25 , 300, 25 ), "Volume: " + Mathf.Round( newVolume ) );
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b

	}
}
