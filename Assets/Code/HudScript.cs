﻿using UnityEngine;
using System.Collections;

public class HudScript : MonoBehaviour {

<<<<<<< HEAD
=======
    public GameObject score1;
	public GameObject score2;
	public GameObject currentTeam;

	public Texture2D redStones;
	public Texture2D blueStones;

    private int redOffset = 40;
    private int blueOffset = 40;

    private int stonesLeftTeamOne;
    private int stonesLeftTeamTwo;
    
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
	// Use this for initialization
	void Start () {
	
	}
<<<<<<< HEAD
	
	// Update is called once per frame
	void Update () {
	
	}
=======

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
		int[] scores = GameManager.Singleton().getScore ();
		score1.guiText.text = "Team 1: " + scores[0].ToString();
		score2.guiText.text = "Team 2: " + scores[1].ToString();
	}

	//A function that updates the display for the current team
	private void UpdateCurrentPlayer() {
        if (GameManager.Singleton().IsTeamOne()) {
			currentTeam.guiText.text = "Team 1's turn";
			currentTeam.guiText.material.color = new Color32( 248, 18, 18, 255 );
		} else {
			currentTeam.guiText.text = "Team 2's turn";
			currentTeam.guiText.material.color = new Color32( 131, 177, 210, 255 );
		}
	}

    private void UpdateStonesLeft() {
        stonesLeftTeamOne = GameManager.Singleton().TeamOneStonesLeft();
        stonesLeftTeamTwo = GameManager.Singleton().TeamTwoStonesLeft();
    }

	private void UpdateStoneCounter() {
		//Draw stones for team one
        for (int i = 0; i <= stonesLeftTeamOne; i++) {
			GUI.DrawTexture(new Rect( ( Screen.width / 2 ) - 100 + redOffset, ( Screen.height / 2 ) - 260, 32, 32 ), redStones);
			redOffset += 40;
		}

        //Draw stones for team two
		for (int i = 0; i <= stonesLeftTeamTwo; i++) {
			GUI.DrawTexture(new Rect( ( Screen.width / 2 ) - 100 + blueOffset, ( Screen.height / 2 ) - 220, 32, 32 ), blueStones);
			blueOffset += 40;
		}

		redOffset = 0;
		blueOffset = 0;
	}
	
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
}
