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
<<<<<<< HEAD
    public GameManager.eTeam team;
    public Camera rockCamera;

    private bool inSupply;

    Vector3 BULLSEYE_POSITION = new Vector3(10.0f, 10.0f, 10.0f);

    void Start()
    {
        inSupply = true;
    }

    public float DistanceFromBullseye()
    {
        return (transform.position - GetBullseyePos()).magnitude;
    }


    private Vector3 GetBullseyePos()
    {
        return BULLSEYE_POSITION;
    }

    public void Fire()
    {
        inSupply = false;
    }

    public bool InSupply()
    {
        return inSupply;
    }
=======
    // --------------------------------------
    // game objects
    // --------------------------------------
    private Player player;
    public Camera rockCamera;

    // --------------------------------------
    // local variables
    // --------------------------------------
    public GameManager.eTeam team;
    private bool inSupply, isPickedUp;
    private Vector3 bullseyePos =           new Vector3(10.0f, 10.0f, 10.0f);
    private float   FRICTION_PERCENTAGE =   0.1f;

    // --------------------------------------
    // functions
    // --------------------------------------

    void Start() {
        inSupply =              true;
        isPickedUp =            false;
        player =                FindObjectOfType<Player>();
        //bullseyePos =         FindObjectOfType<Bullseye>();  <---- this will be useful when we have the proper arena positions sorted out
        // NEED TO ADD: GameObject of class bullseye / else use findobjectoftag and tag it, whichever is easier
    }

    void Update() {
        //ApplyFriction();
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

    private void ApplyFriction() {
        //Vector3 v = rigidbody.velocity.magnitude;

        if (rigidbody.velocity.magnitude > 0) {
          //  rigidbody.ApplyForce(-v.x * FRICTION_PERCENTAGE, -v.y * FRICTION_PERCENTAGE, -v.z * FRICTION_PERCENTAGE);
        }
    }
>>>>>>> 14f9fe656e9ec71239b6dfc1ae68cad2e788a17b
}