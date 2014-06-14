/*
	FILE:			Brusher.cs
	AUHTOR:			Krz, Chris
	PROJECT:		Geri-Lynn Ramsey's Xtreme Curling 2014
	SOUNDTRACK:		Bullitnuts - Dark Horse

	DESCRIPTION:	The brush control script
*/


using UnityEngine;
using System.Collections;

public class Brusher : MonoBehaviour {
	//x/y mouse position coords
    private float mousePositionX;
	
	//get scrub motion
    private bool leftClip;
    private bool rightClip;

    private float farLeft = 0f;
    private float farRight = 0f;
    private bool movingLeft = true;
    
    private float timeElapse = 0;   //elapsed time
    private float scrubPercent = 0;
    private int NUMBER_OF_SCRUBS_PER_SECOND_TO_ACHIEVE_100_PERCENT = 8;

    void Start () {
	}

/*	//debug gui
	void OnGUI () {
		// Make a background box
		/
		GUI.Box(new Rect(50,50,160,160),
                "X: " + MousePositionX.ToString()
		        + "\n leftClip: " + leftClip
		        + "\n rightClip: " + rightClip
		        + "\n timeElapse: " + timeElapse
		        + "\n scrub%: " + scrubPercent);
	}                                               */
	
	void Update () {
		ScrubX();

        AnimationSpeed(scrubPercent);
        SetFriction(scrubPercent);
	}
	
	//change scrub animation speed by % of passed value.
    private void AnimationSpeed(float scrubPercent) {
		//play vareity of animations
		if (scrubPercent > 0) {
			//max out animation - so no crazy speeds
			animation["npc_action"].speed = scrubPercent * 2f;
            animation.CrossFade("npc_action");
		} else {
			//no motion - play idle animation
			animation.CrossFade("npc_running");
		}
	}

    private void SetFriction(float scrubPercent) {
		GameManager.Singleton().SetFriction(1.0f - scrubPercent);
	}

	private void ScrubX() {
        //get mouse x position
        mousePositionX = Input.mousePosition.x;
        timeElapse += Time.deltaTime;

        if (mousePositionX < farLeft) {
            if (!movingLeft) {
                timeElapse = 0;
            }
            farLeft = mousePositionX;
            farRight = mousePositionX;
            movingLeft = true;
        } else if (mousePositionX > farRight) {
            if (movingLeft) {
                timeElapse = 0;
            } 
            farLeft = mousePositionX;
            farRight = mousePositionX;
            movingLeft = false;
        }

        scrubPercent = Mathf.Clamp(1f - (timeElapse * NUMBER_OF_SCRUBS_PER_SECOND_TO_ACHIEVE_100_PERCENT),0f,1f);
	}
}
