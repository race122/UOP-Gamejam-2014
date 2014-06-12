<<<<<<< HEAD
﻿/*
=======
/*
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
	File:			GameManager.cs
	Author:			Krz, Dan, Zack
	Project:		Curling Game
	Soundtrack:		Station 90 Show 13: Simon Heartfield and Manni Dee
	Description:	Manages the state of the game.
*/

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
<<<<<<< HEAD
=======
	
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
	public static float volume = checkVolume();
    static Vector2 BULLSEYE_POSITION = new Vector2( 10.0f, 10.0f );
    private static int team1score = 0, team2score = 0;
    public GameObject stonesDeposit;
    public Camera playerCam;
    public Camera rockCam;
    private eGameState mGameState;
    
    public enum eGameState {
        ePlayer = 0,
        eRock,
    }

    void Awake() {
        Screen.lockCursor = true;
        Screen.showCursor = false;
		ChangeState (eGameState.ePlayer);
	}

<<<<<<< HEAD
	private static float  checkVolume() {
		if ( PlayerPrefs.GetFloat ( "volume " ) < 0.0f ) 
		{
			PlayerPrefs.SetFloat ( "volume", 1.0f );
			PlayerPrefs.Save ();
			volume = 1.0f;
=======
	private static float checkVolume() {
		if ( PlayerPrefs.HasKey("volume")) 
		{
			volume = PlayerPrefs.GetFloat( "volume" );
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
			return volume;
		}
		else 
		{
<<<<<<< HEAD
			volume = PlayerPrefs.GetFloat( "volume" );
=======
			volume = 1.0f;
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
		}

		return volume;
	}
	
	void Update() {	
		if ( Input.GetKeyUp( KeyCode.Escape ) ) {
			Application.LoadLevel( "mainMenu" );
		}
	}

	public enum eTeam {
		TEAM_1,
		TEAM_2
	}

	private static GameManager gm;
	public static GameManager Singleton() {
		if ( !gm ) {
			gm = FindObjectOfType<GameManager>();
		}

		return gm;
	}

    public void UpdateScores() {
        eTeam winningTeam = GetRoundWinner();
        float nmeDistanceFromBullseye = GetNMEClosestToBullseye( winningTeam );
        GivePoints( winningTeam, nmeDistanceFromBullseye );
<<<<<<< HEAD
=======
        print(winningTeam);
        print(team1score);
        print(team2score);
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
    }

    private eTeam GetRoundWinner() {
        eTeam winningTeam = 0;
        float winningDistance = 99999.9f;

        foreach ( Rock rock in FindObjectsOfType<Rock>() ) {
            if ( rock.DistanceFromBullseye() < winningDistance ) {
                winningTeam = rock.team;
                winningDistance = rock.DistanceFromBullseye();
            }
        }

        return winningTeam;
    }

    private float GetNMEClosestToBullseye( eTeam winningTeam ) {
        float closestToBullseye = 99999.9f;
        foreach ( Rock rock in FindObjectsOfType<Rock>() ) {
            if ( rock.team != winningTeam ) {
                if ( rock.DistanceFromBullseye() < closestToBullseye ) {
                	closestToBullseye = rock.DistanceFromBullseye();
                }
            }
        }

        return closestToBullseye;
    }

	private void GivePoints( eTeam winningTeam, float nmeDistanceFromBullseye ) {
        int points = 0;

        foreach ( Rock rock in FindObjectsOfType<Rock>() ) {
            if ( rock.team == winningTeam ) {
                if ( rock.DistanceFromBullseye() < nmeDistanceFromBullseye ) {
                    points++;
                }
            }
        }

        GiveWinningTeamPoints( winningTeam, points );
    }

    private void GiveWinningTeamPoints( eTeam team, int points ) {
        switch ( team ) {
            case eTeam.TEAM_1:
<<<<<<< HEAD
                {
                    team1score += points;
                    break;
                }
            case eTeam.TEAM_2:
                {
                    team2score += points;
                    break;
                }
=======
            {
                team1score += points;
                break;
            }
                
            case eTeam.TEAM_2:
            {
                team2score += points;
                break;
            }
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
        }
    }

    public void ChangeState( eGameState state ) {
        mGameState = state;

        if ( state == eGameState.ePlayer ) {
            playerCam.enabled = true;
            rockCam.enabled = false;
        }

        if ( state == eGameState.eRock ) {
            playerCam.enabled = false;
            rockCam.enabled = true;
        }
    }

<<<<<<< HEAD
    public eGameState GetState() {
        return mGameState;
    }

	public static float getVolume() {
		return volume;
	}

	public static void setVolume( float newVolume ) {
=======
    public eGameState getState() {
        return mGameState;
    }

    public float getVolume() {
		return volume;
	}

    public void setVolume(float newVolume) {
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
		volume = newVolume / 100;
		PlayerPrefs.SetFloat( "volume", volume );
		PlayerPrefs.Save();
	}

<<<<<<< HEAD
	public static int[] getScore()
	{
		int[] scores = {team1score, team2score};
		return scores;
	}
=======
    public int[] getScore() {
		int[] scores = {team1score, team2score};
		return scores;
	}

    public bool IsTeamOne() {
        return (FindObjectOfType<Player>().team == eTeam.TEAM_1);
    }


    public int TeamOneStonesLeft() {
        int i = 0;

        foreach (Rock stone in FindObjectsOfType<Rock>())
        {
            if (stone.InSupply() && stone.team == GameManager.eTeam.TEAM_1)
            {
                i++;
            }
        }

        print(i);

        return i;
    }

    public int TeamTwoStonesLeft()
    {
        int i = 0;

        foreach (Rock stone in FindObjectsOfType<Rock>())
        {
            if (stone.InSupply() && stone.team == GameManager.eTeam.TEAM_2)
            {
                i++;
            }
        }

        print(i);

        return i;
    }
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
}