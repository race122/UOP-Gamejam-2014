using UnityEngine;
using System.Collections;

public class EndRoundScript : MonoBehaviour {

	public GUIText roundWinner;
	public GUIText roundNumber;
	public GUIText roundScores;
	//public string buttonName;
	// Use this for initialization
	void Start () {
		//roundWinner.text = "This guy won";
		Screen.lockCursor = false;
		updateRoundNumber ();

		//I think it's more efficient to call GameManagers.getScore method once
		//and then pass array to both functions
		int[] scores = GameManager.getScore ();
		UpdateRoundWinner (scores);
		updateRoundScore (scores);

	}
	
	void OnGUI() {
		if (GUI.Button (new Rect (Screen.width / 2 - 200, Screen.height / 2 + 80, 100, 50), "Next Round")) {
			Application.LoadLevel("RinkMain");
		}
	}

	private void updateRoundNumber() {
		roundNumber.text = "Round: " + GameManager.getRoundNumber ();
	}

	private void updateRoundScore(int[] scores) {
		roundScores.text = "Red Team: " + scores [0] + "\n" + "Blue Team: " + scores [1];
	}
	
	private void UpdateRoundWinner(int[] scores) {
		//int[] scores = GameManager.getScore ();
		if (scores [0] > scores [1]) {
			roundWinner.text = "Red Team wins!";
		} 
		else if (scores [0] < scores [1]) {
			roundWinner.text = "Blue Team wins!";
		} 
		else {
			roundWinner.text = "Round Draw!";
		}
	}
}
