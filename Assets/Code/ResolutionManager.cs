/*
	File:			ResolutionManager.cs
    Author:			Krz
	Project:		Curling Game
 	Soundtrack:		Fnoob Techno Radio
    Description:	Manages the camera view for different aspect ratios
 */

using UnityEngine;
using System.Collections;

public class ResolutionManager : MonoBehaviour {
    public Camera camera;

    void Start () {
        float screenAspectRatio = (float)Screen.width / (float)Screen.height;

        if (screenAspectRatio > 1.77f) {
            camera.orthographicSize = 8.5f;
        } else if (screenAspectRatio > 1.34f) {
            camera.orthographicSize = 10;
        } else if (screenAspectRatio < 1.34f) {
            camera.orthographicSize = 11.25f;
        }
    }
}