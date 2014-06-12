using UnityEngine;
using System.Collections;

public class MouseTest : MonoBehaviour {

	static float _SCRUBTIME = 10f;
<<<<<<< HEAD
	static int _SCREENHEIGHT = Screen.height;
	static int _SCREENWIDTH = Screen.width;

	//y mouse position coords dgsdg
=======

	//y mouse position coords
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
	float MousePositionY;

	//get scrub motion
	bool bottomClip;
	bool topClip;

	//elapsed time
	float timeElapse = 0;
	//Overall scrub elapsed time
	float scrubValue = 0;
	//per scrub elapsed time
	float scrubPercent = 0;
<<<<<<< HEAD
	//start animating
	float scrubFinal = 100;
=======
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b

	// Use this for initialization
	void Start () {
	
	}
	//debug gui
	void OnGUI () {
		// Make a background box
<<<<<<< HEAD
		GUI.Box(new Rect(10,10,100,110), "Y: " + MousePositionY.ToString()
=======
		GUI.Box(new Rect(10,10,100,100), "Y: " + MousePositionY.ToString()
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
		        + "\n topClip: " + topClip
		        + "\n bottomClip: " + bottomClip
		        + "\n timeElapse: " + timeElapse
		        + "\n scrub: " + scrubValue
<<<<<<< HEAD
		        + "\n scrub%: " + scrubPercent
=======
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
		        + "\n scrub%: " + scrubPercent);

	}


	// Update is called once per frame
	void Update () {

		MousePositionY = Input.mousePosition.y;

<<<<<<< HEAD
		//Get clipping
		if (MousePositionY > _SCREENHEIGHT / 2)
=======

		if (MousePositionY > 200)
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
		{
			topClip = true;
			bottomClip = false;
		}
<<<<<<< HEAD
		if (MousePositionY < _SCREENHEIGHT / 2)
=======
		if (MousePositionY < 200)
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
		{
			topClip = false;
			bottomClip = true;
		}

		//get scrub power
		if (topClip == true)
		{
			timeElapse += Time.deltaTime;
<<<<<<< HEAD
			scrubPercent = 10 - (timeElapse * 100);
=======
			scrubPercent = 100 - (timeElapse * 100);
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
			if (scrubPercent < 0)
			{
				scrubPercent = 0;
			}
		}

		//reset
		if (topClip == false)
		{
			//count total scrub elapsed time
			scrubValue += timeElapse;
<<<<<<< HEAD

			scrubFinal += scrubPercent;

=======
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
			//bleed off scrub percent
			if (scrubPercent <= 0)
			{
				scrubPercent = 0;
			}
			else
			{
				scrubPercent -= 0.5f;
<<<<<<< HEAD

			}

=======
			}
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
			//reset
			timeElapse = 0;
		}

<<<<<<< HEAD
		animationSpeed(scrubFinal);

		//bleed off
		if (scrubFinal <= 0)
		{
			scrubFinal = 0;
		}
		else
		{
			scrubFinal -= 0.5f;
		}
	}

	//change scrub animation speed by % of passed value.
	void animationSpeed(float scrubPercent)
	{
		//play vareity of animations
		if ((scrubPercent / 75) > 110f)
		{
			//max out animation - so no crazy speeds
			animation["Running"].speed = 120f;
		}
		else if (scrubPercent > 0)
		{
			//normal
			animation["Running"].speed = scrubPercent / 75;
		}
		else
		{
			//no motion - play idle animation
		}
=======
		animationSpeed(scrubPercent);


	}

	void animationSpeed(float scrubPercent)
	{
		//change scrub animation speed by % of passed value.
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
	}
}
