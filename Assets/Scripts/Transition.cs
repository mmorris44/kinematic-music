﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public GameObject from, to;

    // Set the transition to go from object to object
    public void SetTransition(GameObject from, GameObject to)
    {
        this.from = from;
        this.to = to;

        DrawTransition(from.transform.position, to.transform.position,
                from.GetComponent<Renderer>().bounds.size.x / 2, to.GetComponent<Renderer>().bounds.size.x / 2);
    }

    // Draw in a transition
    private void DrawTransition(Vector3 a, Vector3 b, float aRadius, float bRadius)
    {
        transform.position = (a + b) / 2; // Average the position
        // Adjust position to account for radius difference
        Vector3 adjust = (a - transform.position);
        adjust /= Mathf.Sqrt(adjust.x * adjust.x + adjust.y * adjust.y);
        adjust *= (bRadius - aRadius) / 2;
        transform.position += adjust;

        transform.localScale *= ((a-b).magnitude - aRadius - bRadius); // Scale up to correct distance
        transform.Rotate(0, 0, Mathf.Rad2Deg * Mathf.Atan((b.y - a.y) / (b.x - a.x))); // Rotate to correct orientation
        if (b.x < a.x) transform.Rotate(0, 0, 180); // Flip if meant to be facing left
    }
}