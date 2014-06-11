using UnityEngine;
using System.Collections;

public class MouseTest : MonoBehaviour {

	static float _SCRUBTIME = 10f;

	//y mouse position coords
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

	// Use this for initialization
	void Start () {
	
	}
	//debug gui
	void OnGUI () {
		// Make a background box
		GUI.Box(new Rect(10,10,100,100), "Y: " + MousePositionY.ToString()
		        + "\n topClip: " + topClip
		        + "\n bottomClip: " + bottomClip
		        + "\n timeElapse: " + timeElapse
		        + "\n scrub: " + scrubValue
		        + "\n scrub%: " + scrubPercent);

	}


	// Update is called once per frame
	void Update () {

		MousePositionY = Input.mousePosition.y;


		if (MousePositionY > 200)
		{
			topClip = true;
			bottomClip = false;
		}
		if (MousePositionY < 200)
		{
			topClip = false;
			bottomClip = true;
		}

		//get scrub power
		if (topClip == true)
		{
			timeElapse += Time.deltaTime;
			scrubPercent = 100 - (timeElapse * 100);
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
			//bleed off scrub percent
			if (scrubPercent <= 0)
			{
				scrubPercent = 0;
			}
			else
			{
				scrubPercent -= 0.5f;
			}
			//reset
			timeElapse = 0;
		}

		animationSpeed(scrubPercent);


	}

	void animationSpeed(float scrubPercent)
	{
		//change scrub animation speed by % of passed value.
	}
}
