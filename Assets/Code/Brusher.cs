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
    
    private float timeElapse = 0f;   //elapsed time
    private float scrubPercent = 0f;
    private float scrubDecay = 0f;
    private int NUMBER_OF_SCRUBS_PER_SECOND_TO_ACHIEVE_100_PERCENT = 6;
    private eScrubPlace scrubPlace;


    public enum eScrubPlace {
        eFarLeft = 0,
        eLeft,
        eMiddle,
        eRight,
        eFarRight
    }
    
    // --------------------------------------
    // functions
    // --------------------------------------
    
    void Start () {
        
	}

/*	//debug gui
	void OnGUI () {
		// Make a background box
		
		GUI.Box(new Rect(50,50,160,160),
                "Decay: " + scrubDecay
		        + "\n timeElapse: " + timeElapse
		        + "\n scrub%: " + scrubPercent);
	}                                               */
	
	void Update () {
        if (GameManager.Singleton().GetGameState() == GameManager.eGameState.eRock) {
		    ScrubX();

            //this was a quick fix to keep the scrubbers scrubbing and didnt inturrupt the animation
            scrubDecay += scrubPercent - (Time.deltaTime * 10f);
            scrubDecay = Mathf.Clamp(scrubDecay, 0f, 1f);

            AnimationSpeed(scrubDecay);
            SetFriction(scrubPercent, GameManager.Singleton().GetCursorXPosition());
        }
	}
	
	//change scrub animation speed by % of passed value.
    private void AnimationSpeed(float scrubPercent) {
		//play vareity of animations
		if (scrubPercent > 0.3) {
			animation["npc_action"].speed = 1f;
            animation.CrossFade("npc_action");
        } else {
            //no motion - play idle animation
            animation.CrossFade("npc_running");
        }
	}

    private void SetFriction(float scrubPercent, float xPosition) {
        if (xPosition <= 0.2f) {
            scrubPlace = eScrubPlace.eFarLeft;
        } else if (xPosition <= 0.4f) {
            scrubPlace = eScrubPlace.eLeft;
        } else if (xPosition >= 0.8f) {
            scrubPlace = eScrubPlace.eFarRight; 
        } else if (xPosition >= 0.6f) {
            scrubPlace = eScrubPlace.eRight;
        } else {
            scrubPlace = eScrubPlace.eMiddle;
        }

        GameManager.Singleton().SetFriction(1.0f - scrubPercent, scrubPlace);
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
