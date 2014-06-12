using UnityEngine;
using System.Collections;

public class EndRoundScript : MonoBehaviour {

	public GUIText roundWinner;
	public GUIText roundNumber;
	//public string buttonName;
	// Use this for initialization
	void Start () {
		//roundWinner.text = "This guy won";
		UpdateRoundWinner ();
		updateRoundNumber ();
	}
	
	void OnMouseUp() {
		//if (buttonName == "Next Round") {
		//	print("Next round clicked");
		//}
	}

	public void updateRoundNumber() {
		roundNumber.text = "Round: " + GameManager.getRoundNumber ();
	}
	
	public void UpdateRoundWinner() {
		int[] scores = GameManager.getScore ();
		if (scores [0] > scores [1]) {
			roundWinner.text = "Team 1 won the round";
		} 
		else if (scores [0] < scores [1]) {
			roundWinner.text = "Team 2 won the round";
		} 
		else {
			roundWinner.text = "The round was a draw";
		}
	}
	
	// Update is called once per frame
	//void Update () {
	
	//}
}
