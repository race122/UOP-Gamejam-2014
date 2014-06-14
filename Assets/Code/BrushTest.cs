using UnityEngine;
using System.Collections;

public class BrushTest : MonoBehaviour {
	//x/y mouse position coords
    private float MousePositionX;
	
	//get scrub motion
    private bool leftClip;
    private bool rightClip;
    
    private float timeElapse = 0;   //elapsed time
    private float scrubPercent = 0;
    private bool prevClipWasLeft = false;   //holds last frame clip (left/right state)
    private int NUMBER_OF_SCRUBS_PER_SECOND_TO_ACHIEVE_100_PERCENT = 4;

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
        //get mouse x position
		MousePositionX = Input.mousePosition.x;

        //Get clipping EW
        rightClip = (MousePositionX > Screen.width * 0.5f);
        leftClip = !(MousePositionX > Screen.width * 0.5f);

		//scrub X axis
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
        timeElapse += Time.deltaTime;

        if (!prevClipWasLeft && leftClip) {
            timeElapse = 0;
        }

        if (rightClip && prevClipWasLeft) {
            timeElapse = 0;
        }

        prevClipWasLeft = leftClip;

        scrubPercent = Mathf.Clamp(1f - (timeElapse * NUMBER_OF_SCRUBS_PER_SECOND_TO_ACHIEVE_100_PERCENT),0f,1f);
	}
}
