using UnityEngine;
using System.Collections;

public class EndRoundScript : MonoBehaviour {

	public GUIText roundWinner;
	public GUIText roundNumber;
	public GUIText roundScores;

    private Color32 redTeamTextColor = new Color32(248, 18, 18, 255);
    private Color32 blueTeamTextColor = new Color32(30, 91, 229, 255);

	//public string buttonName;
	// Use this for initialization
	void Start () {
		//roundWinner.text = "This guy won";
		Screen.lockCursor = false;
		Screen.showCursor = true;
		updateRoundNumber ();

		//I think it's more efficient to call GameManagers.getScore method once
		//and then pass array to both functions
		int[] scores = GameManager.GetScore ();
		UpdateRoundWinner (scores);
		updateRoundScore (scores);

	}

	void OnGUI() {
		if (GUI.Button (new Rect (Screen.width / 2 - 200, Screen.height / 2 + 80, 100, 50), "Next Round")) {
			Application.LoadLevel("RinkMain");
		}
	}

	private void updateRoundNumber() {
		roundNumber.text = "Round: " + GameManager.GetRoundNumber();
	}

	private void updateRoundScore(int[] scores) {
		roundScores.text = "RED TEAM: " + scores [0] + "\n" + "BLUE TEAM: " + scores [1];
	}

	private void UpdateRoundWinner(int[] scores) {
		//int[] scores = GameManager.getScore ();
		if (scores [0] > scores [1]) {
            roundWinner.guiText.color = redTeamTextColor;
            roundWinner.guiText.text = "RED TEAM LEADS!";
		}
		else if (scores [0] < scores [1]) {
            roundWinner.guiText.color = blueTeamTextColor;
            roundWinner.guiText.text = "BLUE TEAM LEADS!";
		}
		else {
            roundWinner.guiText.text = "TEAMS ARE TIED!";
		}
	}
}
