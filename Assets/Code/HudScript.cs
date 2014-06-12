using UnityEngine;
using System.Collections;

public class HudScript : MonoBehaviour {
    public GUIText score1;
    public GUIText score2;
    public GUIText currentTeam;

	public Texture2D redStones;
	public Texture2D blueStones;

    private int redOffset = 40;
    private int blueOffset = 40;

    private int stonesLeftTeamOne;
    private int stonesLeftTeamTwo;

    private Color32 redTeamTextColor =	new Color32( 248, 18, 18, 255 );
    private Color32 blueTeamTextColor =	new Color32( 131, 177, 219, 255 );
    
	// Use this for initialization
	void Start() {
	}

	// Update is called once per frame
	void Update () {
        UpdateStonesLeft();
	}

	void OnGUI() {
		UpdateScores ();
		UpdateCurrentPlayer ();
        UpdateStoneCounter ();
	}

	//A function that updates the display for teams current scores
	private void UpdateScores() {
		int[] scores = GameManager.getScore ();
		score1.guiText.text = "Team 1: " + scores[0].ToString();
		score2.guiText.text = "Team 2: " + scores[1].ToString();
	}

	//A function that updates the display for the current team
	// Dan: Surely then, this would be better called "UpdateCurrentTeam()"?
	private void UpdateCurrentPlayer() {
        // if ( GameManager.Singleton().IsTeamOne() ) {
		// 	currentTeam.guiText.text = "Team 1's turn";
		// 	currentTeam.guiText.material.color = new Color32( 248, 18, 18, 255 );
		// } else {
		// 	currentTeam.guiText.text = "Team 2's turn";
		// 	currentTeam.guiText.material.color = new Color32( 131, 177, 210, 255 );
		// }

		// Dan: My way is better...
		currentTeam.guiText.text =				GameManager.Singleton().IsTeamOne() ? "Red team's turn" : "Blue team's turn";
		currentTeam.guiText.material.color =	GameManager.Singleton().IsTeamOne() ? redTeamTextColor : blueTeamTextColor;
	}

    private void UpdateStonesLeft() {
        stonesLeftTeamOne = GameManager.TeamOneStonesLeft();
        stonesLeftTeamTwo = GameManager.TeamTwoStonesLeft();

        // check to see if the player still has the stone
        if (GameManager.Singleton().GetGameState() == GameManager.eGameState.ePlayer) {
            if (GameManager.Singleton().IsTeamOne()) {
                stonesLeftTeamOne++;
            } else {
                stonesLeftTeamTwo++;
            }
        }
    }

	private void UpdateStoneCounter() {
		//Draw stones for team one
        for (int i = 0; i < stonesLeftTeamOne; i++) {
			GUI.DrawTexture(new Rect( ( Screen.width ) * 0.41f + redOffset, ( Screen.height ) * 0.02f, 32, 32 ), redStones);
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

	private void DrawStoneDirection() {
		//GUI.DrawTexture( new Rect( (  ) ) );
		//float theta = Vector3.Dot( Vector3 lhs, Vector3 rhs );
	}
<<<<<<< HEAD

	private void OutputWinner() {

	}
	
=======
>>>>>>> 1e073484e9f5f137c43e5fdcaeea401b6af31988
}
