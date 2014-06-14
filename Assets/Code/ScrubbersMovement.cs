using UnityEngine;
using System.Collections;

public class ScrubbersMovement : MonoBehaviour {
	private Vector3 currentRockPosition;
	
	void Start () {
	}

    // Update is called once per frame
	void Update () {
        UpdateCurrentRockPosition();
        UpdateScrubberPosition();
	}

	private void UpdateCurrentRockPosition() {
		foreach (Rock stone in FindObjectsOfType<Rock>()) {
			if (stone.IsPickedUp() == true || stone.IsFiring() == true) {
				currentRockPosition = stone.GetPosition();
			}
		}
	}

    private void UpdateScrubberPosition() {
        Vector3 difference;

        if (GameManager.Singleton().GetGameState() == GameManager.eGameState.eRock) {
            //elastic
            difference = currentRockPosition;
            
            // offset the scrubbers
            if (name == "ScrubberRight") {
                difference.x += 1.5f;
                difference.z += 3f;
            } else {
                difference.x -= 1.5f;
            }

            //move
            transform.position = difference;
        } else {
            transform.position = new Vector3(0f, 100f, 0f);         // put them in heaven?
        }
    }
}
