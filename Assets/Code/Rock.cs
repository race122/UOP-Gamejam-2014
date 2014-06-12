/*
    File:           Rock.cs
    Author:         Krz
    Project:        Curling Game
    Soundtrack:     Station 90 Show 13: Simon Heartfield and Manni Dee
    Description:    The puck-like objects the players hit around
*/

using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour
{
    // --------------------------------------
    // game objects
    // --------------------------------------
    public Camera rockCamera;

    // --------------------------------------
    // local variables
    // --------------------------------------
    public GameManager.eTeam team;
    private bool inSupply, isPickedUp;
    private Vector3 bullseyePos =           new Vector3(10.0f, 10.0f, 10.0f);
    private float frictionValue;

    private float FRICTION_MAX =            0.5f;
    private float FRICTION_MIN =            0.01f;

    // --------------------------------------
    // functions
    // --------------------------------------
    void Start() {
        inSupply =              true;
        isPickedUp =            false;
        frictionValue =         FRICTION_MAX;
        //bullseyePos =         FindObjectOfType<Bullseye>();  <---- this will be useful when we have the proper arena positions sorted out
        // NEED TO ADD: GameObject of class bullseye / else use findobjectoftag and tag it, whichever is easier
    }

    void Update() {
        UpdateFriction();
    }

    public float DistanceFromBullseye() {
        return (transform.position - bullseyePos).magnitude;
    }

    public void Pickup() {
        inSupply =              false;
        isPickedUp =            true;
    }

    public void Fire() {
        isPickedUp =            false;
        // rigidbody.velocity =    player.rigidbody.velocity;
    }

    public bool InSupply() {
        return inSupply;
    }

    public bool IsPickedUp() {
        return isPickedUp;
    }

    public bool IsMoving()
    {
        return (rigidbody.velocity.magnitude > 0);
    }

    private void UpdateFriction() {
        if (IsMoving()) {
            rigidbody.AddForce(rigidbody.velocity * frictionValue * -1f);
        }
    }

    // Send a value 0-1 to this function to set the friction value 
    // (0 = lowest, 1 = highest)
    public void SetFriction(float friction) {
        frictionValue = Mathf.Clamp(frictionValue * FRICTION_MAX,FRICTION_MIN,FRICTION_MAX);
    }
}