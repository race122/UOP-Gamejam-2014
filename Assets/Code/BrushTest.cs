using UnityEngine;
using System.Collections;

public class BrushTest : MonoBehaviour {
	//x/y mouse position coords
    private float MousePositionX;
	
	//get scrub motion
    private bool leftClip;
    private bool rightClip;
	
	//elapsed time
    private float timeElapse = 0;
	//Overall scrub elapsed time
    private float scrubValue = 0;
	//per scrub elapsed time
    private float scrubPercent = 0;
	//start animating
	private float scrubFinal = 100;

	Vector3 scrubVector = new Vector3(0, 0, 0);

    void Start () {
	}

	//debug gui
	void OnGUI () {
		// Make a background box
		/*
		GUI.Box(new Rect(50,50,160,160), "Y: " + MousePositionY.ToString()
		        + "\n leftClip: " + leftClip
		        + "\n rightClip: " + rightClip
		        + "\n timeElapse: " + timeElapse
		        + "\n scrub: " + scrubValue
		        + "\n scrub%: " + scrubPercent
		        + "\n scrub% / 0.075: " + scrubPercent * 0.75
		        + "\n scrub Vector: " + scrubVector);
		*/
	}
	
	
	// Update is called once per frame
	void Update () {
        //get mouse x position
		MousePositionX = Input.mousePosition.x;

        //Get clipping EW
        rightClip = (MousePositionX > Screen.width / 2);
        leftClip = !(MousePositionX > Screen.width / 2);

		//scrub Y axis
		scrubY();
		
		// cap
		if (scrubFinal >120) {
			scrubFinal = 120.0f;
		}

        //bleed off
		if (scrubFinal <= 0) {
			scrubFinal = 0;
			scrubVector.x = 0;
		} else {
			scrubFinal -= 0.7f;
			scrubVector.x -= 0.5f;
		}

        animationSpeed(scrubFinal);
        setFriction(scrubFinal);
	}
	
	//change scrub animation speed by % of passed value.
	void animationSpeed(float scrubPercent) {
		//play vareity of animations
		if ((scrubPercent * 0.75) > 110f) {
			//max out animation - so no crazy speeds
			animation["npc_action"].speed = 120f;
		} else if (scrubPercent > 0) {
			//normal
			animation.CrossFade("npc_action");
			animation["npc_action"].speed = scrubPercent / 75;
		} else {
			//no motion - play idle animation
			animation.CrossFade("npc_running");
		}
	}

	void setFriction(float scrubPercent) {
		GameManager.Singleton().SetFriction(1.0f - scrubPercent);
	}

	void scrubY() {
		//get scrub power
		if (leftClip == true) {
			timeElapse += Time.deltaTime;
			scrubPercent = 1f - (timeElapse * 1000);
			if (scrubPercent < 0) {
				scrubPercent = 0;
			}
		}
		
		//reset
		if (rightClip == false) {
			//count total scrub elapsed time
			scrubValue += timeElapse;
			
			scrubFinal += scrubPercent;
			scrubVector.x = scrubFinal;
			
			//bleed off scrub percent
			if (scrubPercent <= 0) {
				scrubPercent = 0;
			} else {
				scrubPercent -= 0.5f;
			}
			
			//reset
			timeElapse = 0;
		}
	}

	void setAnimation(float value) {
		scrubFinal = value;
	}
}
