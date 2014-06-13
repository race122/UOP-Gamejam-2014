using UnityEngine;
using System.Collections;

public class ScrubbersMovement : MonoBehaviour {
	private Rock rock;

	Vector3 thePosition;

	//debug gui
	void OnGUI () {
		// Make a background box
		//GUI.Box(new Rect(10,10,160,160), "pos: " + thePosition);
	}
	// Use this for initialization
	void Start () {
		rock = FindObjectOfType<Rock>();
	}
	Vector3 difference;
	// Update is called once per frame
	void Update () {
        PositionUpdate();
		print (thePosition);
		if (rock.IsPickedUp() == true || rock.IsFiring() == true)
		{
			thePosition = rock.transform.position;

		}

		//elastic
		difference =  thePosition - this.transform.position;
		//offset
		difference.x -= 1;
		//move
		this.transform.position += difference * 0.5f;

		//this.transform.position = thePosition;

	}

	private void PositionUpdate() {
		foreach (Rock stone in FindObjectsOfType<Rock>()) {
			if (stone.IsPickedUp()) {
				rock = stone;
				thePosition = rock.GetPosition();
			}
		}
	}
}
