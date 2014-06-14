using UnityEngine;
using System.Collections;

public class HudScript : MonoBehaviour {
    public GameObject score1;
    public GameObject score2;
    public GameObject currentTeam;

	public Texture2D redStones;
	public Texture2D blueStones;

	//public Texture2D hudPointer;
	//public Texture2D hudArc;

    private int redOffset = 40;
    private int blueOffset = 40;

    private int stonesLeftTeamOne;
    private int stonesLeftTeamTwo;

    private Color32 redTeamTextColor =	new Color32( 248, 18, 18, 255 );
    private Color32 blueTeamTextColor =	new Color32( 30, 91, 229, 255 );

	// Use this for initialization
	void Start() {
		//stone = FindObjectsOfType<Rock> ();
	}

	// Update is called once per frame
	void Update () {
        UpdateStonesLeft();
	}

	void OnGUI() {
		UpdateScores ();
		UpdateCurrentTeam ();
        UpdateStoneCounter ();
		//DrawStoneDirection (); //may remove this if we can't get it done in time
        // it wasnt done in time... removed -Krz
	}

	//A function that updates the display for teams current scores
	private void UpdateScores() {
		int[] scores = GameManager.GetScore ();
		score1.guiText.text = "RED TEAM: " + scores[0].ToString();
        score2.guiText.text = "BLUE TEAM: " + scores[1].ToString();
	}

	//A function that updates the display for the current team
	// Dan: Surely then, this would be better called "UpdateCurrentTeam()"?
    // Krz: okay i renamed it for you :p
	private void UpdateCurrentTeam() {
		// Dan: My way is better...
        currentTeam.guiText.enabled =           (GameManager.Singleton().GetGameState() == GameManager.eGameState.ePlayer);     //only display when the play is making their shot
		currentTeam.guiText.text =				GameManager.Singleton().IsTeamOne() ? "RED TEAM MAKE YOUR SHOT" : "BLUE TEAM MAKE YOUR SHOT";
		currentTeam.guiText.material.color =	GameManager.Singleton().IsTeamOne() ? redTeamTextColor : blueTeamTextColor;
	}

    private void UpdateStonesLeft() {
        stonesLeftTeamOne = GameManager.TeamOneStonesLeft();
        stonesLeftTeamTwo = GameManager.TeamTwoStonesLeft();
    }

	private void UpdateStoneCounter() {
		//Draw stones for team one
		//if (stone.InSupply () && stone.team == GameManager.eTeam.TEAM_RED) {}
		for (int i = 0; i < stonesLeftTeamOne; i++) {
			GUI.DrawTexture (new Rect ((Screen.width) * 0.41f + redOffset, (Screen.height) * 0.02f, 32, 32), redStones);
			redOffset += 40;
		}

	        //Draw stones for team two
			for (int i = 0; i < stonesLeftTeamTwo; i++) {
				//GUI.DrawTexture(new Rect( ( Screen.width * 0.5f ) - 100 + blueOffset, ( Screen.height * 0.02f ) - 220, 32, 32 ), blueStones);
				GUI.DrawTexture(new Rect( ( Screen.width ) * 0.41f + blueOffset, ( Screen.height ) * 0.07f, 32, 32 ), blueStones);
				blueOffset += 40;
			}

		redOffset =		0;
		blueOffset =	0;
	}
    /*
	private void DrawStoneDirection() {
		//GUI.DrawTexture( new Rect( (  ) ) );
		//float theta = Vector3.Dot( Vector3 lhs, Vector3 rhs );

		//Work out how to rotate and move pointer dependent on what direction the stone is travelling
		GUI.DrawTexture (new Rect ((Screen.width) * 0.49f, (Screen.height) * 0.81f, 32, 32), hudPointer);
		GUI.DrawTexture (new Rect ((Screen.width) * 0.4f, (Screen.height) * 0.85f, 256, 64), hudArc);
	}
*/
}
