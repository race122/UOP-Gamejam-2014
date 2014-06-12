using UnityEngine;
using System.Collections;

public class CameraTrolley : MonoBehaviour {
    private Player player;
    private Vector3 offset =            new Vector3(0f, 1.5f, 0f);

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = offset + player.transform.position;
	}
}
