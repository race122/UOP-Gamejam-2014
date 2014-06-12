/*
	File:			GameManager.cs
	Author:			Krz, Dan, Zack
	Project:		Curling Game
	Soundtrack:		Station 90 Show 13: Simon Heartfield and Manni Dee
	Description:	Manages the state of the game.
*/

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public static float volume = checkVolume();
    private static int team1score = 0, team2score = 0;
    public GameObject stonesDeposit;
    public Camera playerCam;
    public Camera rockCam;
    private eGameState mGameState;
    private Player player;
    
    public enum eGameState {
        ePlayer = 0,
        eRock,
    }

    void Awake() {
        Screen.lockCursor = true;
        Screen.showCursor = false;
		ChangeState (eGameState.ePlayer);
        player = FindObjectOfType<Player>();
	}

	private static float checkVolume() {
		if ( PlayerPrefs.HasKey("volume") ) 
		{
			volume = PlayerPrefs.GetFloat( "volume" );
			return volume;
		}
		else 
		{
			volume = 1.0f;
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
        GivePoints(winningTeam, GetEnemyClosestToBullseye(winningTeam) );
        Debug.Log("Game Over");
        Debug.Log(winningTeam + "won the game");

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

    private float GetEnemyClosestToBullseye( eTeam winningTeam ) {
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

	private void GivePoints( eTeam winningTeam, float enemyDistanceFromBullseye ) {
        int points = 0;

        foreach ( Rock rock in FindObjectsOfType<Rock>() ) {
            if ( rock.team == winningTeam ) {
                if ( rock.DistanceFromBullseye() < enemyDistanceFromBullseye ) {
                    points++;
                }
            }
        }

        GiveWinningTeamPoints( winningTeam, points );
    }

    private void GiveWinningTeamPoints( eTeam team, int points ) {
        switch ( team ) {
            case eTeam.TEAM_1:
            {
                team1score += points;
                break;
            }
                
            case eTeam.TEAM_2:
            {
                team2score += points;
                break;
            }
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

    public eGameState getState() {
        return mGameState;
    }

	public static float GetVolume() {
		return volume;
	}

    public static void SetVolume(float newVolume) {
		volume = newVolume / 100;
		PlayerPrefs.SetFloat( "volume", volume );
		PlayerPrefs.Save();
	}

	public static int[] getScore() {
		int[] scores = {team1score, team2score};
		return scores;
	}

    public bool IsTeamOne() {
        return (FindObjectOfType<Player>().team == eTeam.TEAM_1);
    }


	public static int TeamOneStonesLeft() {
        int i = 0;

        foreach (Rock stone in FindObjectsOfType<Rock>())
        {
            if (stone.InSupply() && stone.team == GameManager.eTeam.TEAM_1)
            {
                i++;
            }
        }

        return i;
    }

	public static int TeamTwoStonesLeft()
    {
        int i = 0;

        foreach (Rock stone in FindObjectsOfType<Rock>())
        {
            if (stone.InSupply() && stone.team == GameManager.eTeam.TEAM_2)
            {
                i++;
            }
        }

        return i;
    }

    public void SetFriction(float friction) {
        player.SetFriction(friction);
    }
}