using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Ten skrypt przez caly czas przemieszcza statek do przodu (wedlug osi X) */

public class ShipMovement : MonoBehaviour
{
    public float speed;

    private Rigidbody rigidBody;

    private Vector2 movement;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        movement = new Vector2(speed, 0);
    }

    private void FixedUpdate()
    {
        // Poruszamy statek tylko gdy jest on na wodzie
        bool onGround = true;
        foreach (Floater floater in GetComponentsInChildren<Floater>())
        {
            if (!floater.isFloaterGrounded())
            {
                onGround = false;
            }
        }
        if (onGround)
        {
            MoveBoat(movement);
        }
    }

    private void MoveBoat(Vector2 direction)
    {
        rigidBody.AddForce(direction*speed);
    }

}
