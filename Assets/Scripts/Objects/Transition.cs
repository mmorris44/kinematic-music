using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the transform of a transition arrow
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

    // Updates the transform based on the current assigned game objects
    public void UpdateTransform()
    {
        // Reset the scale and rotation to default
        transform.localScale = new Vector3(0.034f, 0.05f, 1);
        transform.rotation = Quaternion.identity;

        SetTransition(from, to);
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

        transform.localScale = new Vector3(transform.localScale.x * ((a - b).magnitude - aRadius - bRadius), 
            transform.localScale.y, transform.localScale.z); // Scale up to correct distance
        transform.Rotate(0, 0, Mathf.Rad2Deg * Mathf.Atan((b.y - a.y) / (b.x - a.x))); // Rotate to correct orientation
        if (b.x < a.x) transform.Rotate(0, 0, 180); // Flip if meant to be facing left
    }
}
